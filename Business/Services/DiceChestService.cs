using System.Security.Cryptography;
using Business.Factories;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;

namespace Business.Services
{

    public class DiceChestService : IChestService<DiceChestServiceParameters>
    {
        private readonly ChestConfigurationOptions _chestConfiguration;
        private readonly IItemFactory<GrenadeMod, GrenadeModFactoryParameters> _grenadeFactory;
        private readonly IItemFactory<Shield, ShieldFactoryParameters> _shieldFactory;
        private readonly IItemFactory<Gun, GunRandomizerFactoryParameters> _gunFactory;
        private readonly IItemFactory<Relic, RelicRandomizerFactoryParameters> _relicFactory;
        private readonly IPotionFinderService _potionFinderService;


        public DiceChestService(IOptions<ChestConfigurationOptions> chestConfiguration, IItemFactory<GrenadeMod, GrenadeModFactoryParameters> grenadeFactory, IItemFactory<Shield, ShieldFactoryParameters> shieldFactory, IItemFactory<Gun, GunRandomizerFactoryParameters> gunFactory, IItemFactory<Relic, RelicRandomizerFactoryParameters> relicFactory, IPotionFinderService potionFinderService)
        {
            _chestConfiguration = chestConfiguration.Value ?? throw new ArgumentNullException(nameof(chestConfiguration));
            _grenadeFactory = grenadeFactory ?? throw new ArgumentNullException(nameof(grenadeFactory));
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
            _relicFactory = relicFactory ?? throw new ArgumentNullException(nameof(relicFactory));
            _potionFinderService = potionFinderService ?? throw new ArgumentNullException(nameof(potionFinderService));
        }

        public Chest OpenChest(DiceChestServiceParameters parameters)
        {
            var roll1 = RandomNumberGenerator.GetInt32(1, _chestConfiguration.DiceChests.Length+1);
            var roll2 = RandomNumberGenerator.GetInt32(1, _chestConfiguration.DiceChests.Length+1);
            var maxRoll = Math.Max(roll1, roll2);
            var spec = _chestConfiguration.DiceChests.First(s => s.Roll == maxRoll);


            var items = new List<Item>();
            
            foreach (var weaponSpec in spec.WeaponSpecs)
            {
                var weaponBuildParameters = new GunRandomizerFactoryParameters { PlayerLevel = parameters.PlayerLevel, Rarity = weaponSpec.Rarity, AllowPrefixes = parameters.AllowPrefixes, AllowRedTexts = parameters.AllowPrefixes };
  
                if (weaponSpec.IsFavored)
                {
                    weaponBuildParameters.GunType = parameters.GunType;
                }
                
                items.Add(_gunFactory.Manufacture(weaponBuildParameters).Item);
            }

            foreach (var potionSpec in spec.PotionSpecs)
            {
                items.Add(_potionFinderService.GetPotion(potionSpec));
            }

            foreach (var relicSpec in spec.RelicSpecs)
            {
                items.Add(_relicFactory.Manufacture(new RelicRandomizerFactoryParameters { Rarity = relicSpec.Rarity }).Item);
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

            return new Chest
            {
                Roll = maxRoll,
                Items = items
            };
        }
    }
}