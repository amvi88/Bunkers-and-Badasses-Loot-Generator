using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class Spell : Item
    {
        public string Name {get; set;}

        public Prefix Prefix {get; set;}

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

        public string Effect {get; set;}

        public string GuildEffect {get; set;}

        public string Source { get; set; }

        public Rarity Rarity { get; set; } = Rarity.Common;
    }
}
