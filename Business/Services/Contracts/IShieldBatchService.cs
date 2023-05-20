using Models.Builder;

namespace Business.Services
{
    public interface IShieldBatchService
    {
        IEnumerable<Shield> GenerateBatch(ShieldBatchServiceParameters parameters);
    }
}