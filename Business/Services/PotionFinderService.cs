using Business.Factories;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class PotionFinderService : IPotionFinderService
    {
        private readonly PotionConfigurationOptions _potionConfiguration;
        private readonly IItemFactory<Potion, BaseFactoryParameters> _potionFactory;

        public PotionFinderService(IOptions<PotionConfigurationOptions> potionConfiguration, IItemFactory<Potion, BaseFactoryParameters> potionFactory)
        {
            _potionConfiguration = potionConfiguration.Value ?? throw new ArgumentNullException(nameof(potionConfiguration));
            _potionFactory = potionFactory ?? throw new ArgumentNullException(nameof(potionFactory));
        }

        public Potion GetPotion(ChestPotionSpec potionSpec)
        {
            if (potionSpec.IsRandomPotion)
            {
                return _potionFactory.Manufacture(new BaseFactoryParameters()).Item;
            }

            var potionConfigSpec = _potionConfiguration.RegularPotions.First(x => x.Name.Equals(potionSpec.Name, StringComparison.InvariantCultureIgnoreCase));
            return new Potion
            {
                Name = potionConfigSpec.Name,
                Effect = potionConfigSpec.Effect,
                Cost = potionConfigSpec.Cost
            };
        }

    }
}