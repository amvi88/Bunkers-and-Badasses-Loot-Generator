using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
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

        public ItemWrapper<Potion> Manufacture(BaseFactoryParameters factoryParameters)
        {
            var roll = RandomNumberGenerator.GetInt32(1,111);    
            var specs = _potionConfiguration.RegularPotions.First(x => x.MinRoll <= roll && roll <= x.MaxRoll);

            var potion = new Potion
            {
                Name  = specs.Name,
                Effect = specs.Effect,                
                Cost = specs.Cost,
                Rarity = specs.Rarity
            };


            if (specs.IsElemental.GetValueOrDefault())
            {
                var element = GetRandomElement();

                potion.IsElemental = true;
                potion.Element = element;
                potion.Effect = string.Format(specs.Effect, element);
            }

            else if (specs.RollTinasPotions.GetValueOrDefault())
            {
                var potionIndex = RandomNumberGenerator.GetInt32(0,20) + specs.RollModifier.GetValueOrDefault();
                var ttPotion = _potionConfiguration.TinyTinasPotions[potionIndex];

                potion.IsTinyTinasPotion = true;
                potion.Name = ttPotion.Name;
                potion.Effect = ttPotion.Effect;
            }

            return new ItemWrapper<Potion>
            {
                DiceRolls = new DiceRoll[]
                {
                    new DiceRoll
                    {
                        DiceType = "d100",
                        Result = roll
                    }
                },
                Item = potion
            };
        }

        private Element GetRandomElement(){
            var elementalTypes = Enum.GetValues(typeof(Element));
            return (Element)elementalTypes.GetValue(RandomNumberGenerator.GetInt32(1,elementalTypes.Length));
        }
    }    
}