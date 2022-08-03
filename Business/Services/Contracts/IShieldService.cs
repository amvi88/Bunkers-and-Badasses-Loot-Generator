using Models.Builder;

namespace Business.Services
{
    public interface IShieldService
    {
        ItemWrapper<Shield> RandomizeShield(ShieldFactoryParameters parameters);
    }
}