using Business.Models.Common;
using Business.Models.Config;

namespace Business.Models
{
    public class Gun : Item
    {
        public Rarity Rarity{get; set; }

        public Element Element {get; set; }

        public int Level {get; set; }

        public string Damage {get; set; }

        public int Range { get; set; }

        public string Name { get; set; }

        public RedText RedText {get; set;}

        public List<string> Bonuses {get; set;}

        public Dictionary<string, WeaponHits> HitsByAccuracy { get; internal set; }
    }
}