using Models.Common;

namespace Models.Config
{
    public class WeaponCustomizationOptions 
    {
        public Rarity[][] RarityMatrix {get; set;}

        public Dictionary<Rarity, int> PrefixChance  {get; set;}

        public Prefix[] Prefixes {get; set;}   

        public Dictionary<Rarity, int> RedTextChance  {get; set;}

        public RedText[] RedText {get; set;}   

        public ElementalChance[] ElementalTable { get; set; }
    }
}
