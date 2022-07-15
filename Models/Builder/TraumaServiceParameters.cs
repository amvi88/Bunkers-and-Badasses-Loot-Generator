using Models.Common;

namespace Models.Builder
{
    public class TraumaServiceParameters : BaseFactoryParameters
    {
        public TraumaType TraumaType {get; set;} = TraumaType.Temporary;
    }
}