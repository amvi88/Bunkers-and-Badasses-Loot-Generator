using System.Security.Cryptography;
using Business.Factories;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class EnemyDropService : IChestService<EnemyDropServiceParameters>
    {
        private readonly ChestConfigurationOptions _chestConfiguration;
        private readonly IItemFactory<GrenadeMod, GrenadeModFactoryParameters> _grenadeFactory;
        private readonly IItemFactory<Shield, ShieldFactoryParameters> _shieldFactory;
        private readonly IItemFactory<Gun, GunFactoryParameters> _gunFactory;
        private readonly IItemFactory<Relic, RelicFactoryParameters> _relicFactory;
        private readonly IPotionFinderService _potionFinderService;


        public EnemyDropService(IOptions<ChestConfigurationOptions> chestConfiguration, IItemFactory<GrenadeMod, GrenadeModFactoryParameters> grenadeFactory, IItemFactory<Shield, ShieldFactoryParameters> shieldFactory, IItemFactory<Gun, GunFactoryParameters> gunFactory, IItemFactory<Relic, RelicFactoryParameters> relicFactory, IPotionFinderService potionFinderService)
        {
            _chestConfiguration = chestConfiguration.Value ?? throw new ArgumentNullException(nameof(chestConfiguration));
            _grenadeFactory = grenadeFactory ?? throw new ArgumentNullException(nameof(grenadeFactory));
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
            _relicFactory = relicFactory ?? throw new ArgumentNullException(nameof(relicFactory));
            _potionFinderService = potionFinderService ?? throw new ArgumentNullException(nameof(potionFinderService));
        }

        public Chest OpenChest(EnemyDropServiceParameters parameters)
        {
            var items = new List<Item>();

            for (int indexLoot = 1; indexLoot <= parameters.LootPiles; indexLoot++)
            {
                var col = RandomNumberGenerator.GetInt32(0, 4);

                var spec = _chestConfiguration.EnemyDrops[indexLoot - 1][col];

                if (spec.Gold > 0)
                {
                    items.Add(new Gold { Amount = spec.Gold });
                }

                foreach (var weaponSpec in spec.WeaponSpecs)
                {
                    var weaponBuildParameters = new GunFactoryParameters { PlayerLevel = parameters.PlayerLevel, AllowPrefixes = parameters.AllowPrefixes, AllowRedTexts = parameters.AllowPrefixes };
                    items.Add(_gunFactory.Manufacture(weaponBuildParameters).Item);
                }

                foreach (var potionSpec in spec.PotionSpecs)
                {
                    items.Add(_potionFinderService.GetPotion(potionSpec));
                }

                foreach (var relicSpec in spec.RelicSpecs)
                {
                    items.Add(_relicFactory.Manufacture(new RelicFactoryParameters { Rarity = relicSpec.Rarity }).Item);
                }

                for (int index = 0; index < spec.AmountOfGrenadeMods; index++)
                {
                    items.Add(_grenadeFactory.Manufacture(new GrenadeModFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item);
                }

                for (int index = 0; index < spec.AmountOfGrenades; index++)
                {
                    items.Add(new Grenade());
                }

                for (int index = 0; index < spec.AmountOfShieldMods; index++)
                {
                    items.Add(_shieldFactory.Manufacture(new ShieldFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item);
                }
            }

            return new Chest
            {
                Items = items
            };
        }
    }
}