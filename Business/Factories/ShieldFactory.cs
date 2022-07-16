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

        public override ItemWrapper<Shield> Manufacture(ShieldFactoryParameters parameters)
        {
            var chosenGuild = GetGuild(parameters.Guild, ItemType.Shield, out var roll);
            var specs = chosenGuild.ShieldSpecs.First(x => x.MinLevel <= parameters.PlayerLevel && parameters.PlayerLevel <= x.MaxLevel);

            var shield = new Shield
            {
                Level = parameters.PlayerLevel,
                Guild =  chosenGuild.Name,
                Capacity = specs.Capacity,
                RechargeRate = specs.RechargeRate,
                Effect = specs.Effect                
            };

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