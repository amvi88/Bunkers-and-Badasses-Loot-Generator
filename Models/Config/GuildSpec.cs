using Models.Common;

namespace Models.Config
{
    public class GuildSpec
    {
        public string Name { get; set; }

        public string AlternateName { get; set; }

        public List<ShieldSpec> ShieldSpecs { get; set; } 

        public List<GrenadeSpec> GrenadeSpecs { get; set; } 

        public List<WeaponSpec> WeaponSpecs { get; set; } 

        public List<SpellSpec> SpellSpecs { get; set; }

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