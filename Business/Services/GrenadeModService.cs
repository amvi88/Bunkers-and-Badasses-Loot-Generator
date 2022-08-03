using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class GrenadeModService : IGrenadeModService
    {
        private readonly IItemFactory<GrenadeMod, GrenadeModFactoryParameters> grenadeModFactory;

        public GrenadeModService(IItemFactory<GrenadeMod, GrenadeModFactoryParameters> grenadeModFactory)
        {
            this.grenadeModFactory = grenadeModFactory ?? throw new ArgumentNullException(nameof(grenadeModFactory));
        }

        public ItemWrapper<GrenadeMod> RandomizeGrenadeMod(GrenadeModFactoryParameters parameters)
        {
            return grenadeModFactory.Manufacture(parameters);
        }
    }
}