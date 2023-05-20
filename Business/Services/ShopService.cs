using Business.Factories;
using Models.Builder;
using Models.Config;

namespace Business.Services
{

    public class ShopService : IShopService
    {
        private readonly ChestConfigurationOptions _chestConfiguration;
        private readonly IItemFactory<GrenadeMod, GrenadeModFactoryParameters> _grenadeFactory;
        private readonly IItemFactory<Shield, ShieldFactoryParameters> _shieldFactory;
        private readonly IItemFactory<Gun, GunRandomizerFactoryParameters> _gunFactory;
        private readonly IItemFactory<Potion, BaseFactoryParameters> _potionFactory;


        public ShopService(IItemFactory<GrenadeMod, GrenadeModFactoryParameters> grenadeFactory, IItemFactory<Shield, ShieldFactoryParameters> shieldFactory, IItemFactory<Gun, GunRandomizerFactoryParameters> gunFactory, IItemFactory<Potion, BaseFactoryParameters> potionFactory)
        {
            _grenadeFactory = grenadeFactory ?? throw new ArgumentNullException(nameof(grenadeFactory));
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
            _potionFactory = potionFactory ?? throw new ArgumentNullException(nameof(potionFactory));
        }

        public IEnumerable<Item> GenerateInventory(ShopServiceParameters parameters)
        {
            for (int index = 0; index < parameters.AmountOfGuns; index++)
            {
                var weaponBuildParameters = new GunRandomizerFactoryParameters { PlayerLevel = parameters.PlayerLevel };
                yield return _gunFactory.Manufacture(weaponBuildParameters).Item;
            }

            for (int index = 0; index < parameters.AmountOfGrenadeMods; index++)
            {
                yield return _grenadeFactory.Manufacture(new GrenadeModFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item;
            }

            for (int index = 0; index < parameters.AmountOfShieldMods; index++)
            {
                yield return _shieldFactory.Manufacture(new ShieldFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item;
            }

            for (int index = 0; index < parameters.AmountOfPotions; index++)
            {
                yield return _potionFactory.Manufacture(null).Item;
            }
        }
    }
}