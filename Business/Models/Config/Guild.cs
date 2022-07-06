using Business.Models.Common;

namespace Business.Models.Config
{
    public class Guild
    {
        public string Name { get; set; }

        public List<ShieldSpec> ShieldSpecs { get; set; } 

        public List<GrenadeSpec> GrenadeSpecs { get; set; } 

        public List<WeaponSpec> WeaponSpecs { get; set; } 

        public ItemType SupportedItemTypes { get; set; }

        public GunType? SupportedGunTypes { get; set; }

        public bool CanBuild(ItemType itemType)
        {
            return SupportedItemTypes.HasFlag(itemType);
        }

        public bool CanBuild(GunType gunType)
        {
            return SupportedGunTypes?.HasFlag(gunType) ?? false;
        }
    }

}