using Business.Models;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class ShieldFactory : IItemFactory<Shield>
    {
        private readonly GuildConfigurationOptions _guildConfiguration;

        public ShieldFactory(IOptions<GuildConfigurationOptions> guildOptions)
        {
            _guildConfiguration = guildOptions.Value ?? throw new ArgumentException(nameof(guildOptions));
        }

        public Shield Manufacture(int playerLevel, Rarity? rarity)
        {
            var guildsThatProduceShields = _guildConfiguration.Guilds.Where(x => x.CanBuild(ManufacturerItemType.Shield));
            var chosenGuild = guildsThatProduceShields.ElementAt(RandomNumberGenerator.GetInt32(0, guildsThatProduceShields.Count()));
            var specs = chosenGuild.ShieldSpecs.First(x => x.MinLevel <= playerLevel && playerLevel <= x.MaxLevel);

            var shield = new Shield
            {
                Level = playerLevel,
                Guild =  chosenGuild.Name,
                Capacity = specs.Capacity,
                RechargeRate = specs.RechargeRate,
                Effect = specs.Effect                
            };

            return shield;            
        }
    }
}