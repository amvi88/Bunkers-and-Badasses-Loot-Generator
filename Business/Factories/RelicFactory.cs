using Models;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class RelicFactory : IItemFactory<Relic, RelicFactoryParameters>
    {
        private readonly RelicConfigurationOptions _relicConfiguration;

        public RelicFactory(IOptions<RelicConfigurationOptions> relicConfiguration)
        {
            _relicConfiguration = relicConfiguration.Value ?? throw new ArgumentException(nameof(relicConfiguration));
        }

        public ItemWrapper<Relic> Manufacture(RelicFactoryParameters factoryParameters)
        {
            var maxRoll = 101;
            var minRoll = 1;

            if (factoryParameters.Rarity.HasValue)
            {
                maxRoll = _relicConfiguration.RelicSpecs.Where(x => x.Rarity == factoryParameters.Rarity).Max(x => (x.MaxRoll ?? x.Roll).GetValueOrDefault()) + 1;
                minRoll = _relicConfiguration.RelicSpecs.Where(x => x.Rarity == factoryParameters.Rarity).Min(x => (x.MinRoll ?? x.Roll).GetValueOrDefault());
            }

            var roll = RandomNumberGenerator.GetInt32(minRoll, maxRoll);    
            var specs = _relicConfiguration.RelicSpecs.First(x => x.MinRoll <= roll && roll <= x.MaxRoll || x.Roll == roll);

            var preferredClass = _relicConfiguration.Classes[RandomNumberGenerator.GetInt32(0, _relicConfiguration.Classes.Length)];


            return new ItemWrapper<Relic>
            {
                DiceRolls = new DiceRoll[]
                {
                    new DiceRoll
                    {
                        DiceType = "d100",
                        Result = roll
                    }
                },
                Item = new Relic
                {
                    Type  = specs.Type,
                    Effect = specs.Effect,                
                    Rarity = specs.Rarity,
                    ClassEffect = specs.ClassEffect,
                    PreferredClass = preferredClass
                }
            };
        }
    }
}