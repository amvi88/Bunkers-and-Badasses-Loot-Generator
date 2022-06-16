using Business.Models;
using Business.Models.Builder;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class PotionFactory : IItemFactory<Potion, BaseFactoryParameters>
    {
        private readonly PotionConfigurationOptions _potionConfiguration;

        public PotionFactory(IOptions<PotionConfigurationOptions> potionConfiguration)
        {
            _potionConfiguration = potionConfiguration.Value ?? throw new ArgumentException(nameof(potionConfiguration));
        }

        public Potion Manufacture(BaseFactoryParameters factoryParameters)
        {
            var roll = RandomNumberGenerator.GetInt32(1,101);    
            var specs = _potionConfiguration.RegularPotions.First(x => x.MinRoll <= roll && roll <= x.MaxRoll);

            var potion = new Potion
            {
                Name  = specs.Name,
                Effect = specs.Effect,                
                Cost = specs.Cost
            };

            if (specs.RollTinasPotions.GetValueOrDefault())
            {
                var potionIndex = RandomNumberGenerator.GetInt32(0,20) + specs.RollModifier.GetValueOrDefault();
                var ttPotion = _potionConfiguration.TinyTinasPotions[potionIndex];

                potion.IsTinyTinasPotion = true;
                potion.Name = ttPotion.Name;
                potion.Effect = ttPotion.Effect;
            }

            return potion; 
        }
    }
}