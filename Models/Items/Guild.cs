using Models.Common;

namespace Models.Builder
{
    public class Guild
    {
        public string Name { get; set; }

        public string AlternameName { get; set; }
        public ItemType SupportedItemTypes { get; set; }

        public GunType? SupportedGunTypes { get; set; }
        
        public bool CanBuild(ItemType itemType)
        {
            return SupportedItemTypes.HasFlag(itemType);
        }

        public bool CanProduceGunType(GunType gunType)
        {
            return SupportedGunTypes?.HasFlag(gunType) ?? false;
        }
    }
}
