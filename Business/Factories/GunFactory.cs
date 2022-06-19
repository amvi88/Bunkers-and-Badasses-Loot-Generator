using Business.Models;
using Business.Models.Builder;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GunFactory : GuildDrivenFactory<Gun, GunFactoryParameters>
    {
        private readonly WeaponCustomizationOptions _weaponCustomization;
        private readonly WeaponArchetypesOptions _weaponArchetypesOptions;

        public GunFactory(IOptions<GuildConfigurationOptions> guildOptions, IOptions<WeaponCustomizationOptions> weaponCustomizationOptions, IOptions<WeaponArchetypesOptions> weaponArchetypesOptions)
            : base(guildOptions)
        {
            _weaponCustomization = weaponCustomizationOptions.Value ?? throw new ArgumentException(nameof(weaponCustomizationOptions));
            _weaponArchetypesOptions = weaponArchetypesOptions.Value ?? throw new ArgumentNullException(nameof(weaponArchetypesOptions));
        }
       
        public override Gun Manufacture(GunFactoryParameters builderArguments)
        {
            
            var rarity = GetRarity(builderArguments.Rarity);  
            var gunType = GetGunType(builderArguments.GunType);
            var chosenGuild = GetGuild(builderArguments.Guild, ItemType.Gun);
             
            // Create a gun based on Guild specs
            var specs = chosenGuild.WeaponSpecs.First(x => x.Rarity == rarity);

            var gun = new Gun
            {
                Level = builderArguments.PlayerLevel,
                Guild =  chosenGuild.Name,
                Element = Element.None,
                Rarity = rarity,
                Bonuses = new List<string>{ $"(Guild) {specs.Bonus}" },
                GunType = gunType.ToString()                         
            };

            // Roll for elemental type if any and bonus damage if it may be
            if (specs.IsElemental)
            {
                var elementalValues = Enum.GetValues(typeof(Element));

                var elementValues = RollElement(rarity, specs.IncreasedElementalRollPercentage);
                gun.Element = elementValues.Item1 ?? Element.None;

                if (! string.IsNullOrWhiteSpace(elementValues.Item2) && elementValues.Item1 != Element.None)
                {
                    gun.Bonuses.Append( $"(Elemental) +{elementValues.Item2} {elementValues.Item1} DMG");
                }
            }

            // Get archetype specs for weapons
            var archetype = _weaponArchetypesOptions.Archetypes.First(x => x.GunType == gunType);
            if (!string.IsNullOrEmpty(archetype.Bonus))
            {
                gun.Bonuses.Add($"({gunType.GetDescription()}) {archetype.Bonus}");
            }

            gun.Name = GetName(archetype);

            var gunStats = archetype.WeaponSpecs.First(x => x.MinLevel <= builderArguments.PlayerLevel && builderArguments.PlayerLevel <= x.MaxLevel);
            gun.Range  = gunStats.Range;
            gun.Damage = gunStats.Damage;
            gun.HitsByAccuracy = gunStats.HitsByAccuracy;  
            
            // Check for prefix
            var calculatedPrefixChance = RandomNumberGenerator.GetInt32(1, 100);
            if ( calculatedPrefixChance <= _weaponCustomization.PrefixChance[rarity])
            {
                var prefix = _weaponCustomization.Prefixes[RandomNumberGenerator.GetInt32(0, _weaponCustomization.Prefixes.Length)];
                gun.Bonuses.Add($"(Prefix) {prefix.Effect}");
                gun.Name = $"{prefix.Name} {gun.Name}";
            }

            var calculatedRedTextChance = RandomNumberGenerator.GetInt32(1, 100);
            if (calculatedRedTextChance  <= _weaponCustomization.RedTextChance[rarity])
            {
                var redText = _weaponCustomization.RedText[RandomNumberGenerator.GetInt32(0, _weaponCustomization.RedText.Length)];
                gun.Bonuses.Add($"(RedText) {redText.Effect}"); 
                gun.RedText = redText;
            }

            return gun;
        }

        private string GetName(WeaponArchetype archetype)
        {
            return archetype.Names[RandomNumberGenerator.GetInt32(0, archetype.Names.Length)];
        }

        private (Element?, string) RollElement(Rarity? rarity, int? rollModifier)
        {
            var percentileRoll = Math.Min(RandomNumberGenerator.GetInt32(1, 101)+rollModifier.GetValueOrDefault() , 100);
            var elementalSpec = _weaponCustomization.ElementalTable.Where(x => x.MinxRoll <= percentileRoll && percentileRoll <= x.MaxRoll).First();
            var index = rarity == null? 0 : (int) rarity;
            return(elementalSpec.Elements[index], elementalSpec.ExtraDamage[index]);
        }

        private Rarity GetRarity(Rarity? rarity)
        {
            if (! rarity.HasValue)
            {
                var rowNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix.Length);
                var colNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomization.RarityMatrix[0].Length);
                return _weaponCustomization.RarityMatrix[rowNumber][colNumber];
            }

            return rarity.GetValueOrDefault();
        }

        private GunType GetGunType(GunType? gunType)
        {
            if (! gunType.HasValue)
            {
                var gunTypeValues = Enum.GetValues(typeof(GunType));
                return  (GunType)gunTypeValues.GetValue(RandomNumberGenerator.GetInt32(1,gunTypeValues.Length));
            }

            return gunType.GetValueOrDefault();
        }
    }
}