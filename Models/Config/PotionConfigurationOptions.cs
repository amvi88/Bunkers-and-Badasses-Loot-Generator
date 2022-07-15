using Models.Common;

namespace Models.Config
{
    public class PotionConfigurationOptions
    {
        public RegularPotion[] RegularPotions { get; set; }
        public PotionSpec[] TinyTinasPotions { get; set; }
    }
}