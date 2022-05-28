using Business.Models.Common;

namespace Business.Models
{
    public class Gun : Item
    {
        public Rarity Rarity{get; set; }

        public Element? Element {get; set; }

        public int Level {get; set; }

        public string Damage {get; set; }

        public int Range { get; set; }

        public GunType GunType { get; set; }
    }
}