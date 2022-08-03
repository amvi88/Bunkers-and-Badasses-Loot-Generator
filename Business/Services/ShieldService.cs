using Business.Factories;
using Models.Builder;

namespace Business.Services
{

    public class ShieldService : IShieldService
    {
        private readonly IItemFactory<Shield, ShieldFactoryParameters> shieldFactory;

        public ShieldService(IItemFactory<Shield, ShieldFactoryParameters> shieldFactory)
        {
            this.shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
        }

        public ItemWrapper<Shield> RandomizeShield(ShieldFactoryParameters parameters)
        {
            return shieldFactory.Manufacture(parameters);
        }
    }
}