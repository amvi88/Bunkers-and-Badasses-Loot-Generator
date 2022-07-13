using Business.Models.Builder;
using Business.Models.Common;

namespace Business.Services
{
    public interface ITraumatizingService
    {
        List<Trauma> GetTrauma(TraumaServiceParameters parameters);
    }
}