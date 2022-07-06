using Business.Models.Common;

namespace Business.Models.Config
{
    public class WeaponSpec
    {
        public Rarity Rarity {get; set;}
        public bool IsElemental { get; set; }
        public string Bonus {get; set;}
        public string Effect {get; set;}        
        public int? IncreasedElementalRollPercentage {get; set;}
    }
}