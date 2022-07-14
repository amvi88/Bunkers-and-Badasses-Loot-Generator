using System.ComponentModel.DataAnnotations;
using Business.Models.Common;

namespace Business.Models.Builder
{
    public class DiceChestServiceParameters : BaseFactoryParameters
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;} = 1;

        public GunType GunType {get; set;}

        public bool AllowPrefixes {get; set; }

        public bool AllowRedTexts {get; set; }
    }
}