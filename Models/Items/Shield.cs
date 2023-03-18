using Models.Builder;
using Models.Common;

namespace Models.Builder
{
    public class Shield : Item
    {
        public int Level {get; set;}

        public string Name {get; set;}

        public int Capacity {get; set;}

        public int RechargeRate {get; set;}  

        public string Effect {get; set;}

        public string Source { get; set; }

        public Rarity Rarity { get; set; } = Rarity.Common;
    }
}
