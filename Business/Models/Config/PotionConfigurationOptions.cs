using Business.Models.Common;

namespace Business.Models.Config
{
    public class PotionConfigurationOptions
    {
        public RegularPotion[] RegularPotions { get; set; }
        public PotionSpec[] TinyTinasPotions { get; set; }
    }
}