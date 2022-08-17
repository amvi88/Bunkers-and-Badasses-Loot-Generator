using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GrenadeModFactory : GuildDrivenFactory<GrenadeMod,GrenadeModFactoryParameters>
    {

        public GrenadeModFactory(IOptions<GuildConfigurationOptions> guildOptions)
        : base (guildOptions)
        {
        }

        public override ItemWrapper<GrenadeMod> Manufacture(GrenadeModFactoryParameters factoryParameters)
        {
            var chosenGuild = GetGuild(factoryParameters.Guild, ItemType.Grenade, out var roll);
            var specs = chosenGuild.GrenadeSpecs.First(x => x.MinLevel <= factoryParameters.PlayerLevel & factoryParameters.PlayerLevel <= x.MaxLevel);

            var grenade = new GrenadeMod
            {
                Level = factoryParameters.PlayerLevel,
                Guild =  chosenGuild.Name,
                AlternateGuildName = chosenGuild.AlternateName,
                GrenadeType = specs.GrenadeType,
                Damage = specs.Damage,
                Effect = specs.Effect,
                Element = Element.None               
            };

            
            if (factoryParameters.Element.HasValue)
            {
                grenade.Element = factoryParameters.Element.GetValueOrDefault();
            }
            else if (specs.IsElemental.GetValueOrDefault())
            {
                var elementalValues = Enum.GetValues(typeof(Element));
                grenade.Element = (Element)elementalValues.GetValue(RandomNumberGenerator.GetInt32(1,elementalValues.Length));
            }

            return new ItemWrapper<GrenadeMod>
            {
                DiceRolls = new DiceRoll[]
                {
                    new DiceRoll
                    {
                        DiceType = "d8",
                        Result = roll

                    }
                },
                Item = grenade
            };            
        }
    }
}