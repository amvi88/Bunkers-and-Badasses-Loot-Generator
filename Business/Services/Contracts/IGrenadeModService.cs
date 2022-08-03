using Models.Builder;

namespace Business.Services
{
    public interface IGrenadeModService
    {
        ItemWrapper<GrenadeMod> RandomizeGrenadeMod(GrenadeModFactoryParameters parameters);
    }
}