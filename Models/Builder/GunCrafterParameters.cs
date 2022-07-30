using System.ComponentModel.DataAnnotations;
using Models.Common;
using Models.Config;

namespace Models.Builder
{
    public class GunCrafterParameters
    {
        public Rarity Rarity {get; set; } = Rarity.Common;

        public Element Element {get; set; } = Element.None;

        [Range(1,30)]
        public int Level {get; set; } = 1;
       
        public int DamageDice {get; set; } = 1;
        public string DamageDiceType {get; set; } = "d4";

        public int ElementalDamageDice {get; set; } = 0;
        public string ElementalDamageDiceType {get; set; } = "d4";
        public int Range { get; set; }

        public string Name { get; set; }

        public GunType GunType { get; set; } = GunType.CombatRifle;

        public string RedTextName {get; set;} = string.Empty;

        public string RedTextEffect {get; set;}

        public string PrefixName {get; set;} = string.Empty;

        public string PrefixEffect {get; set;} = string.Empty;
        
        public string Guild { get; set; }

        public string GuildBonus {get; set;}

        public string GuildEffect {get; set;}

        public string WeaponTypeBonus {get; set;}

        public List<WeaponHits> HitsByAccuracy { get; set; }

        public string ImageUrl { get; set; }
    }
}