using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class ChestServiceParameters : BaseFactoryParameters
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;} = 1;

        public GunType GunType {get; set;} = GunType.CombatRifle;

        public string Guild  {get; set;}

        public bool AllowPrefixes {get; set; }

        public bool AllowRedTexts {get; set; }
    }
}