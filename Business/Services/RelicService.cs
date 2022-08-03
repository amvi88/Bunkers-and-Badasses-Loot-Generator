using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class RelicService : IRelicService
    {
        private readonly IItemFactory<Relic, RelicRandomizerFactoryParameters> relicFactory;

        public RelicService(IItemFactory<Relic, RelicRandomizerFactoryParameters> relicFactory)
        {
            this.relicFactory = relicFactory ?? throw new ArgumentNullException(nameof(relicFactory));
        }

        public ItemWrapper<Relic> RandomizeRelic(RelicRandomizerFactoryParameters parameters)
        {
            return relicFactory.Manufacture(parameters);
        }
    }
}