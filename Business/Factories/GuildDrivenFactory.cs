using Business.Models;
using Business.Models.Builder;
using Business.Models.Common;
using Business.Models.Config;
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

        public abstract I Manufacture(P factoryParameters);

        protected Guild GetGuild(string guildName, ItemType itemType)
        {
            var guildsThatProduceItemType = _guildConfiguration.Guilds.Where(x => x.CanBuild(itemType));

            if (! string.IsNullOrWhiteSpace(guildName))
            {
                return guildsThatProduceItemType.First(x => x.Name.Equals(guildName, StringComparison.InvariantCultureIgnoreCase));
            }

            return guildsThatProduceItemType.ElementAt(RandomNumberGenerator.GetInt32(0, guildsThatProduceItemType.Count()));
        }
    }
}