using System.ComponentModel.DataAnnotations;
using Business.Models.Common;

namespace Business.Models.Builder
{
    public class GrenadeFactoryParameters : BaseFactoryParameters
    {

        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;}

        public string Guild {get; set;}

        public Rarity? Rarity {get; set;}

        public Element? Element {get; set;}
    }
}