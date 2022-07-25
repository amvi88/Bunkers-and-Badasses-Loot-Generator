using Models.Builder;

namespace Models.Builder
{
    public class Shield : Item
    {
        public int Level {get; set;}

        public string Name {get; set;}

        public int Capacity {get; set;}

        public int RechargeRate {get; set;}  

        public string Effect {get; set;}
        public string ImageUrl { get; set; }
        public string Source { get; set; }
    }
}
