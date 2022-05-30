using Business.Models;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class GrenadeFactory : IItemFactory<Grenade>
    {
        private readonly GuildConfigurationOptions _guildConfiguration;

        public GrenadeFactory(IOptions<GuildConfigurationOptions> guildOptions)
        {
            _guildConfiguration = guildOptions.Value ?? throw new ArgumentException(nameof(guildOptions));
        }

        public Grenade Manufacture(int playerLevel, Rarity? rarity)
        {
            var guildsThatProduceGrenades = _guildConfiguration.Guilds.Where(x => x.CanBuild(ManufacturerItemType.Grenade));
            var chosenGuild = guildsThatProduceGrenades.ElementAt(RandomNumberGenerator.GetInt32(0, guildsThatProduceGrenades.Count()));
            var specs = chosenGuild.GrenadeSpecs.First(x => x.MinLevel <= playerLevel && playerLevel <= x.MaxLevel);

            var grenade = new Grenade
            {
                Level = playerLevel,
                Guild =  chosenGuild.Name,
                GrenadeType = specs.GrenadeType,
                Damage = specs.Damage,
                Effect = specs.Effect                
            };

            if (specs.IsElemental.GetValueOrDefault())
            {
                var elementalValues = Enum.GetValues(typeof(Element));
                grenade.Element = (Element)elementalValues.GetValue(RandomNumberGenerator.GetInt32(1,elementalValues.Length));
            }

            return grenade;
            
        }
    }
}