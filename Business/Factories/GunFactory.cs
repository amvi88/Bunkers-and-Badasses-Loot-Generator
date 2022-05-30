using Business.Models;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GunFactory : IGunFactory
    {
        private readonly GuildConfigurationOptions _guildConfiguration;
        private readonly WeaponCustomizationOptions _weaponCustomization;
        private readonly WeaponArchetypesOptions _weaponArchetypesOptions;

        public GunFactory(IOptions<GuildConfigurationOptions> guildOptions, IOptions<WeaponCustomizationOptions> weaponCustomizationOptions, IOptions<WeaponArchetypesOptions> weaponArchetypesOptions)
        {
            _guildConfiguration = guildOptions.Value ?? throw new ArgumentException(nameof(guildOptions));
            _weaponCustomization = weaponCustomizationOptions.Value ?? throw new ArgumentException(nameof(weaponCustomizationOptions));
            _weaponArchetypesOptions = weaponArchetypesOptions.Value ?? throw new ArgumentNullException(nameof(weaponArchetypesOptions));
        }
       
        public Gun Manufacture(int playerLevel, Rarity? rarity)
        {
            var gunTypeValues = Enum.GetValues(typeof(GunType));
            var gunType =  (GunType)gunTypeValues.GetValue(RandomNumberGenerator.GetInt32(1,gunTypeValues.Length));
            return Manufacture(playerLevel, rarity, gunType);
        }

        public Gun Manufacture(int playerLevel, Rarity? rarity, GunType gunType)
        {
            if (! rarity.HasValue)
            {
                rarity = RollRarity();
            }

            var guildsThatProduceSpecificGunType = _guildConfiguration.Guilds.Where(x => x.CanBuild(ManufacturerItemType.Gun) && x.CanBuild(gunType));
            var chosenGuild = guildsThatProduceSpecificGunType.ElementAt(RandomNumberGenerator.GetInt32(0, guildsThatProduceSpecificGunType.Count()));
            
            // Create a gun based on Guild specs
            var specs = chosenGuild.WeaponSpecs.First(x => x.Rarity == rarity);

            var gun = new Gun
            {
                Level = playerLevel,
                Guild =  chosenGuild.Name,
                Element = Element.None,
                Rarity = rarity.GetValueOrDefault(),
                Bonus = specs.Bonus                            
            };

            // Roll for elemental type if any and bonus damage if it may be
            if (specs.IsElemental)
            {
                var elementalValues = Enum.GetValues(typeof(Element));

                var elementValues = RollElement(rarity, specs.IncreasedElementalRollPercentage);
                gun.Element = elementValues.Item1;

                if (! string.IsNullOrWhiteSpace(elementValues.Item2))
                {
                    gun.ExtraDamage = elementValues.Item2;
                }
            }

            // Get arhetype specs for weapons
            var archetype = _weaponArchetypesOptions.Archetypes.First(x => x.GunType == gunType);
            if (!string.IsNullOrEmpty(archetype.Bonus))
            {
                gun.WeaponTypeBonus = archetype.Bonus;
            }

            var gunStats = archetype.WeaponSpecs.First(x => x.MinLevel <= playerLevel && playerLevel <= x.MaxLevel);
            gun.Range  = gunStats.Range;
            gun.Damage = gunStats.Damage;
            gun.HitsByAccuracy = gunStats.HitsByAccuracy;  
            gun.WeaponType = gunType.ToString(); 


            // Check for prefix
            var calculatedPrefixChance = RandomNumberGenerator.GetInt32(1, 100);
            if ( calculatedPrefixChance <= _weaponCustomization.PrefixChance[rarity.GetValueOrDefault()])
            {
                gun.Prefix = _weaponCustomization.Prefixes[RandomNumberGenerator.GetInt32(0, _weaponCustomization.Prefixes.Length)];
            }

            var calculatedRedTextChance = RandomNumberGenerator.GetInt32(1, 100);
            if (calculatedRedTextChance  <= _weaponCustomization.RedTextChance[rarity.GetValueOrDefault()])
            {
                gun.RedText = _weaponCustomization.RedText[RandomNumberGenerator.GetInt32(0, _weaponCustomization.RedText.Length)];
            }

            return gun;
        }

        private (Element?, string) RollElement(Rarity? rarity, int? rollModifier)
        {
            var percentileRoll = Math.Min(RandomNumberGenerator.GetInt32(1, 101)+rollModifier.GetValueOrDefault() , 100);
            var elementalSpec = _weaponCustomization.ElementalTable.Where(x => x.MinxRoll <= percentileRoll && percentileRoll <= x.MaxRoll).First();
            var index = rarity == null? 0 : (int) rarity;
            return(elementalSpec.Elements[index], elementalSpec.ExtraDamage[index]);
        }

        private Rarity? RollRarity()
        {
            var rowNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix.Length);
            var colNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix[0].Length);
            return _weaponCustomization.RarityMatrix[rowNumber][colNumber];
        }
    }
}