using Models.Builder;

namespace Business.Services
{
    public interface IGunBatchService
    {
        IEnumerable<Gun> GenerateBatch(GunBatchServiceParameters parameters);
    }
}