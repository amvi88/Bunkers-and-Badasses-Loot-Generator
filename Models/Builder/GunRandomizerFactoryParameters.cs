using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class GunRandomizerFactoryParameters : BaseFactoryParameters
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;} = 1;

        public string Guild {get; set;}

        public Rarity? Rarity {get; set;}

        public Element? Element {get; set;}

        public GunType? GunType {get; set; }

        public bool AllowPrefixes {get; set; }

        public bool AllowRedTexts {get; set; }

        public bool BatchMode {get; set;}

        public int BatchSize {get; set;} = 1;


    }
}