using System.Security.Cryptography;
using Business.Factories;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;

namespace Business.Services
{

    public class CacheRollService : IChestService<CacheRollServiceParameters>
    {
        private readonly ChestConfigurationOptions _chestConfiguration;
        private readonly IItemFactory<Grenade, GrenadeFactoryParameters> _grenadeFactory;
        private readonly IItemFactory<Shield, ShieldFactoryParameters> _shieldFactory;
        private readonly IItemFactory<Gun, GunFactoryParameters> _gunFactory;
        private readonly IItemFactory<Relic, RelicFactoryParameters> _relicFactory;
        private readonly IPotionFinderService _potionFinderService;


        public CacheRollService(IOptions<ChestConfigurationOptions> chestConfiguration, IItemFactory<Grenade, GrenadeFactoryParameters> grenadeFactory, IItemFactory<Shield, ShieldFactoryParameters> shieldFactory, IItemFactory<Gun, GunFactoryParameters> gunFactory, IItemFactory<Relic, RelicFactoryParameters> relicFactory, IPotionFinderService potionFinderService)
        {
            _chestConfiguration = chestConfiguration.Value ?? throw new ArgumentNullException(nameof(chestConfiguration));
            _grenadeFactory = grenadeFactory ?? throw new ArgumentNullException(nameof(grenadeFactory));
            _shieldFactory = shieldFactory ?? throw new ArgumentNullException(nameof(shieldFactory));
            _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
            _relicFactory = relicFactory ?? throw new ArgumentNullException(nameof(relicFactory));
            _potionFinderService = potionFinderService ?? throw new ArgumentNullException(nameof(potionFinderService));
        }

        public Chest OpenChest(CacheRollServiceParameters parameters)
        {
            var amoutOfDice = Math.Min((int)parameters.CacheRollSize + (parameters.GoldFarmerBonus ? 2 : 0), 6);
            var maxNumber = amoutOfDice * 6 + 1;

            var roll = RandomNumberGenerator.GetInt32(1, maxNumber);
            var spec = _chestConfiguration.CacheRolls.First(s => s.Roll == roll);

            var items = new List<Item>();

            if (spec.Gold > 0)
            {
                items.Add(new Gold { Amount = spec.Gold });
            }


            foreach (var weaponSpec in spec.WeaponSpecs)
            {
                var weaponBuildParameters = new GunFactoryParameters { PlayerLevel = parameters.PlayerLevel, Rarity = weaponSpec.Rarity, AllowPrefixes = parameters.AllowPrefixes, AllowRedTexts = parameters.AllowPrefixes };
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

            for (int index = 0; index < spec.AmountOfGrenades; index++)
            {
                items.Add(_grenadeFactory.Manufacture(new GrenadeFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item);
            }

            for (int index = 0; index < spec.AmountOfShieldMods; index++)
            {
                items.Add(_shieldFactory.Manufacture(new ShieldFactoryParameters { PlayerLevel = parameters.PlayerLevel }).Item);
            }

            return new Chest
            {
                Roll = roll,
                Items = items
            };
        }
    }
}