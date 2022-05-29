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
        private readonly GuildConfiguration _guildConfiguration;
        private readonly WeaponCustomization _weaponCustomization;

        public GunFactory(IOptions<GuildConfiguration> guildOptions, IOptions<WeaponCustomization> weaponCustomizationOptions)
        {
            _guildConfiguration = guildOptions.Value ?? throw new ArgumentException(nameof(guildOptions));
            _weaponCustomization = weaponCustomizationOptions.Value ?? throw new ArgumentException(nameof(weaponCustomizationOptions));
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
            var specs = chosenGuild.WeaponSpecs.First(x => x.Rarity == rarity);

            var gun = new Gun
            {
                Level = playerLevel,
                Guild =  chosenGuild.Name,
                Element = Element.None,
                Rarity = rarity.GetValueOrDefault()                              
            };

            if (specs.IsElemental)
            {
                var elementalValues = Enum.GetValues(typeof(Element));

                var elementValues = RollElement(rarity, specs.IncreasedElementalRollPercentage);
                gun.Element = elementValues.Item1;
                gun.ExtraDamage = elementValues.Item2;
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