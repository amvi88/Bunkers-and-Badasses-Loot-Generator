using System.ComponentModel.DataAnnotations;

namespace Models.Builder
{
    public class ShopServiceParameters : BatchServiceParameters
    {
        [Required]
        [Range(1, 32)]
        public int AmountOfGuns { get; set; } = 1;

        [Required]
        [Range(1, 32)]
        public int AmountOfShieldMods { get; set; } = 1;

        [Required]
        [Range(1, 32)]
        public int AmountOfGrenadeMods { get; set; } = 1;

        [Required]
        [Range(1, 32)]
        public int AmountOfPotions { get; set; } = 1;
    }
}