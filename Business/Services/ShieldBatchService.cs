using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class ShieldBatchService : IShieldBatchService
    {
        private readonly IItemFactory<Shield, ShieldFactoryParameters> _shieldFactory;

        public ShieldBatchService(IItemFactory<Shield, ShieldFactoryParameters> shieldFactory)
        {
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
        }

        public IEnumerable<Shield> GenerateBatch(ShieldBatchServiceParameters parameters)
        {
            for (int index = 0; index < parameters.BatchSize; index++)
            {
                yield return _shieldFactory.Manufacture(new ShieldFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item;
            }
        }
    }
}