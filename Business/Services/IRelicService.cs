using Models.Builder;

namespace Business.Services
{
    public interface IRelicService
    {
        ItemWrapper<Relic> RandomizeRelic(RelicRandomizerFactoryParameters parameters);
    }
}