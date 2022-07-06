using Business.Models.Builder;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GrenadeFactory : GuildDrivenFactory<Grenade,GrenadeFactoryParameters>
    {

        public GrenadeFactory(IOptions<GuildConfigurationOptions> guildOptions)
        : base (guildOptions)
        {
        }

        public override Grenade Manufacture(GrenadeFactoryParameters factoryParameters)
        {
            var chosenGuild = GetGuild(factoryParameters.Guild, ItemType.Grenade);
            var specs = chosenGuild.GrenadeSpecs.First(x => x.MinLevel <= factoryParameters.PlayerLevel & factoryParameters.PlayerLevel <= x.MaxLevel);

            var grenade = new Grenade
            {
                Level = factoryParameters.PlayerLevel,
                Guild =  chosenGuild.Name,
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

            return grenade;            
        }
    }
}