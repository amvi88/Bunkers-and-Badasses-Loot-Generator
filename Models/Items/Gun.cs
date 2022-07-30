using System.ComponentModel.DataAnnotations;
using Models.Common;
using Models.Config;

namespace Models.Builder
{
    public class Gun : Item
    {
        public Rarity Rarity{get; set; }

        public Element Element {get; set; }

        [Range(1,30)]
        public int Level {get; set; } = 1;

        public string Damage {get; set; }
        
        public int AmountOfDice {
            get {
                if (string.IsNullOrWhiteSpace(Damage))
                {
                    return 0;
                }

                return int.TryParse(Damage.Substring(0,1), out var result)? result : 0;
            }
        }
        
        public string DiceType => string.IsNullOrWhiteSpace(Damage)? string.Empty :  Damage.Substring(1);

        public int ExtraAmountOfDice
        { 
            get {
                if (string.IsNullOrWhiteSpace(ExtraDamage))
                {
                    return 0;
                }

                return int.TryParse(ExtraDamage.Substring(0,1), out var result)? result : 0;
            }
        }

        public string ExtraDiceType =>  string.IsNullOrWhiteSpace(ExtraDamage)? string.Empty : ExtraDamage.Substring(1);

        public int Range { get; set; }

        public string Name { get; set; }

        public GunType GunType { get; set; }

        public RedText RedText {get; set;}

        public Prefix Prefix { get; set; }
 
        public string GunArchetypeBonus { get; set; }
 
        public string ElementalBonus { get; set; }
 
        public string GuildBonus { get; set; }
 
        public List<WeaponHits> HitsByAccuracy { get; set; }
 
        public string ExtraDamage { get; set; }

        public string Source { get; set; }

        public string ImageUrl { get; set; }
    }
}