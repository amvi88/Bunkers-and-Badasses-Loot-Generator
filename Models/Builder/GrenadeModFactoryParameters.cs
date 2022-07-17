using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class GrenadeModFactoryParameters : BaseFactoryParameters
    {

        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;} = 1;

        public string Guild {get; set;}

        public Rarity? Rarity {get; set;}

        public Element? Element {get; set;}
    }
}