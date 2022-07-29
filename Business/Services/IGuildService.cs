using Models.Builder;
using Models.Common;

namespace Business.Services
{
    public interface IGuildService
    {
        List<Guild> GetGuilds();
        List<Guild> GetGuildsThatProduce(ItemType itemType);
        List<Guild> GetGuildsThatProduceGunType(GunType? gunType);
        GuildWeaponModifier GetWeaponModifiers(string guildName, Rarity rarity);
    }
}