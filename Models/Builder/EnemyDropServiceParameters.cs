using System.ComponentModel.DataAnnotations;

namespace Models.Builder
{
    public class EnemyDropServiceParameters : ChestServiceParameters
    {
        [Range(1,6)]
        [Required]
        public int LootPiles {get; set;} = 1;
    }
}