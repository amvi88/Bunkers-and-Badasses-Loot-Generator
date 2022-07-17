using Models.Common;

namespace Models.Builder
{
    public class CacheRollServiceParameters : ChestServiceParameters
    {
        public CacheRollSize CacheRollSize {get; set;} = CacheRollSize.Tiny;
        public bool GoldFarmerBonus {get; set;} 
    }
}