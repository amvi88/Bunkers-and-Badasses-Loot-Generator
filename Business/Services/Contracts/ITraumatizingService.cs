using Models.Builder;
using Models.Common;

namespace Business.Services
{
    public interface ITraumatizingService
    {
        List<Trauma> GetTrauma(TraumaServiceParameters parameters);
    }
}