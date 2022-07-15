namespace Models.Config
{
    public class RegularPotion : PotionSpec
    {

        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        public int Cost { get; set; }
        public bool? RollTinasPotions { get; set; }
        public int? RollModifier { get; set;}
        public bool? IsElemental { get; set;}
    }
}