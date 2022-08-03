using Business.Factories;
using Models.Builder;

namespace Business.Services
{

    public class GunService : IGunService
    {
        private readonly IItemFactory<Gun, GunRandomizerFactoryParameters> gunFactory;

        public GunService(IItemFactory<Gun, GunRandomizerFactoryParameters> gunFactory)
        {
            this.gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        }

        public ItemWrapper<Gun> RandomizeGun(GunRandomizerFactoryParameters parameters)
        {
            return gunFactory.Manufacture(parameters);
        }
    }
}