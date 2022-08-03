using Models.Builder;

namespace Business.Services
{
    public interface IGunService
    {
        ItemWrapper<Gun> RandomizeGun(GunRandomizerFactoryParameters parameters);
    }
}