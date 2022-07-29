using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class GunBatchService : IGunBatchService
    {        private readonly IItemFactory<Gun, GunRandomizerFactoryParameters> _gunFactory;

        public GunBatchService(IItemFactory<Gun, GunRandomizerFactoryParameters> gunFactory)
        {
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        }

        public IEnumerable<Gun> GenerateBatch(GunBatchServiceParameters parameters)
        {
            for (int index = 0; index < parameters.BatchSize; index++)
            {
                yield return _gunFactory.Manufacture(new GunRandomizerFactoryParameters { PlayerLevel = parameters.PlayerLevel, AllowPrefixes = true, AllowRedTexts = true }).Item;
            }
        }
    }
}