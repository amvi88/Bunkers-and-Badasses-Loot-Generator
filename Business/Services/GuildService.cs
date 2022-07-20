using Microsoft.Extensions.Options;
using Models.Builder;
using Models.Common;
using Models.Config;

namespace Business.Services
{

    public class GuildService : IGuildService
    {
        private readonly GuildConfigurationOptions _guildConfigurationOptions;

        public GuildService(IOptions<GuildConfigurationOptions> guildConfigurationOptions)
        {
            _guildConfigurationOptions = guildConfigurationOptions.Value ?? throw new ArgumentNullException(nameof(guildConfigurationOptions));
        }

        public List<Guild> GetGuilds()
        {
            return _guildConfigurationOptions.Guilds.Select(g => new Guild
            {
                Name = g.Name,
                SupportedGunTypes = g.SupportedGunTypes,
                SupportedItemTypes = g.SupportedItemTypes
            }).ToList();
        }

        public List<Guild> GetGuildsThatProduce(ItemType itemType)
        {
            return _guildConfigurationOptions.Guilds
            .Where(g => g.CanBuild(itemType))
            .Select(g => new Guild
            {
                Name = g.Name,
                SupportedGunTypes = g.SupportedGunTypes,
                SupportedItemTypes = g.SupportedItemTypes
            }).ToList();
        }

        public List<Guild> GetGuildsThatProduceGunType(GunType? gunType)
        {
            var guildsThatProduceGuns = _guildConfigurationOptions.Guilds
            .Where(g => g.CanBuild(ItemType.Gun))
            .Select(g => new Guild
            {
                Name = g.Name,
                SupportedGunTypes = g.SupportedGunTypes,
                SupportedItemTypes = g.SupportedItemTypes
            });

            if (gunType == null)
            {
                return guildsThatProduceGuns.ToList();
            }

            return guildsThatProduceGuns.Where(g => g.CanProduceGunType(gunType.GetValueOrDefault())).ToList();
        }
    }
}