using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class GunBatchServiceParameters
    {
        [Required]
        [Range(1, 30)]
        public int PlayerLevel {get; set;} = 1;
        
        public int BatchSize {get; set;} = 1;
    }
}