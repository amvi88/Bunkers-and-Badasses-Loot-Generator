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
        public ShieldFactory(IOptions<GuildConfigurationOptions> guildOptions)
        : base(guildOptions)
        {            
        }

        public override Shield Manufacture(ShieldFactoryParameters parameters)
        {
            var chosenGuild = GetGuild(parameters.Guild, ItemType.Shield);
            var specs = chosenGuild.ShieldSpecs.First(x => x.MinLevel <= parameters.PlayerLevel && parameters.PlayerLevel <= x.MaxLevel);

            var shield = new Shield
            {
                Level = parameters.PlayerLevel,
                Guild =  chosenGuild.Name,
                Capacity = specs.Capacity,
                RechargeRate = specs.RechargeRate,
                Effect = specs.Effect                
            };

            return shield;            
        }
    }
}