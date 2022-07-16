using Models.Common;

namespace Models.Config
{
    public class PotionSpec
    {
        public string Name { get; set; }

        public string Effect { get; set; }

        public Rarity Rarity { get; set; } = Rarity.Common;
    }
}