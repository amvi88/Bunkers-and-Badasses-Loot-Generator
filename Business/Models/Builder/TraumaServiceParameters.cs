using Business.Models.Common;

namespace Business.Models.Builder
{
    public class TraumaServiceParameters : BaseFactoryParameters
    {
        public TraumaType TraumaType {get; set;} = TraumaType.Temporary;
    }
}