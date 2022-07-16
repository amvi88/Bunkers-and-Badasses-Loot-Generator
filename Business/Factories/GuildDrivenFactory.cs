using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public abstract class GuildDrivenFactory<I,P>  : IItemFactory<I, P>
        where I: Item 
        where P: BaseFactoryParameters
    {
        private readonly GuildConfigurationOptions _guildConfiguration;

        public GuildDrivenFactory(IOptions<GuildConfigurationOptions> guildOptions)
        {
            _guildConfiguration = guildOptions.Value ?? throw new ArgumentException(nameof(guildOptions));
        }

        public abstract ItemWrapper<I> Manufacture(P factoryParameters);

        protected Guild GetGuild(string guildName, ItemType itemType, out int roll)
        {
            var guildsThatProduceItemType = _guildConfiguration.Guilds.Where(x => x.CanBuild(itemType));

            if (! string.IsNullOrWhiteSpace(guildName))
            {
                roll = 0;
                return guildsThatProduceItemType.First(x => x.Name.Equals(guildName, StringComparison.InvariantCultureIgnoreCase));
            }

            roll = RandomNumberGenerator.GetInt32(0, guildsThatProduceItemType.Count());
            return guildsThatProduceItemType.ElementAt(roll++);
        }


        protected Guild GetGuild(string guildName, ItemType itemType, GunType gunType, out int roll)
        {
            var guildsThatProduceItemType = _guildConfiguration.Guilds.Where(x => x.CanBuild(itemType) && x.CanProduceGunType(gunType));

            if (! string.IsNullOrWhiteSpace(guildName))
            {
                roll = 0;
                return guildsThatProduceItemType.First(x => x.Name.Equals(guildName, StringComparison.InvariantCultureIgnoreCase));
            }

            roll = RandomNumberGenerator.GetInt32(0, guildsThatProduceItemType.Count());
            return guildsThatProduceItemType.ElementAt(roll++);
        }
    }
}