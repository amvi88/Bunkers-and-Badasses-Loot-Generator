using Models;
using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class ShieldFactory : GuildDrivenFactory<Shield, ShieldFactoryParameters>
    {
        private readonly ShieldCustomizationOptions _shieldCustomizationOptions;

        public ShieldFactory(IOptions<GuildConfigurationOptions> guildOptions, IOptions<ShieldCustomizationOptions> shieldCustomizationOptions)
        : base(guildOptions)
        {
            _shieldCustomizationOptions = shieldCustomizationOptions.Value ?? throw new ArgumentNullException(nameof(shieldCustomizationOptions));
        }

        public override ItemWrapper<Shield> Manufacture(ShieldFactoryParameters parameters)
        {
            var chosenGuild = GetGuild(parameters.Guild, ItemType.Shield, out var roll);
            var specs = chosenGuild.ShieldSpecs.First(x => x.MinLevel <= parameters.PlayerLevel && parameters.PlayerLevel <= x.MaxLevel);

            var shield = new Shield
            {
                Level = parameters.PlayerLevel,
                Guild =  chosenGuild.Name,
                AlternateGuildName = chosenGuild.AlternateName,
                Capacity = specs.Capacity,
                RechargeRate = specs.RechargeRate,
                Effect = specs.Effect                
            };

            var matchingShields = _shieldCustomizationOptions.ShieldGallery.Where(x => x.Guild.Equals(shield.Guild, StringComparison.InvariantCultureIgnoreCase));

            if (matchingShields.Any())
            {
                var shieldGallerySpec = matchingShields.ElementAt(RandomNumberGenerator.GetInt32(0, matchingShields.Count()));
                shield.Name = shieldGallerySpec.Name;
                shield.ImageUrl = shieldGallerySpec.ImageUrl;
                shield.Source = shieldGallerySpec.Source;
            }

            return new ItemWrapper<Shield>
            {
                DiceRolls = new DiceRoll[]
                {
                    new DiceRoll
                    {
                        DiceType = "d8",
                        Result = roll

                    }
                },
                Item = shield
            };            
        }
    }
}