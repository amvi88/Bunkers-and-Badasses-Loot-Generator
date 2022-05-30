using Business.Models.Common;
using Business.Models.Config;

namespace Business.Models
{
    public class Gun : Item
    {
        public Rarity Rarity{get; set; }

        public Element? Element {get; set; }

        public int Level {get; set; }

        public string Damage {get; set; }

        public int Range { get; set; }

        public string WeaponType { get; set; }

        public string Prefix {get; set;}

        public string RedText {get; set;}

        public string Bonus {get; set;}
        public string ExtraDamage { get; internal set; }
        public string WeaponTypeBonus { get; internal set; }
        public Dictionary<string, WeaponHits> HitsByAccuracy { get; internal set; }
    }
}