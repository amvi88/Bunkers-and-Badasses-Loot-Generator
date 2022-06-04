using Business.Models;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class RelicFactory : IRelicFactory
    {
        private readonly RelicConfigurationOptions _relicConfiguration;

        public RelicFactory(IOptions<RelicConfigurationOptions> relicConfiguration)
        {
            _relicConfiguration = relicConfiguration.Value ?? throw new ArgumentException(nameof(relicConfiguration));
        }

        public Relic Dig(Rarity? rarity)
        {        
            var maxRoll = 101;
            var minRoll = 1;

            if (rarity.HasValue)
            {
                maxRoll = _relicConfiguration.RelicSpecs.Where(x => x.Rarity == rarity).Max(x => (x.MaxRoll ?? x.Roll).GetValueOrDefault()) + 1;
                minRoll = _relicConfiguration.RelicSpecs.Where(x => x.Rarity == rarity).Min(x => (x.MinRoll ?? x.Roll).GetValueOrDefault());
            }

            var roll = RandomNumberGenerator.GetInt32(minRoll, maxRoll);    
            var specs = _relicConfiguration.RelicSpecs.First(x => x.MinRoll <= roll && roll <= x.MaxRoll || x.Roll == roll);

            var preferredClass = _relicConfiguration.Classes[RandomNumberGenerator.GetInt32(0, _relicConfiguration.Classes.Length)];


            var relic = new Relic
            {
                Type  = specs.Type,
                Effect = specs.Effect,                
                Rarity = specs.Rarity,
                ClassEffect = specs.ClassEffect,
                PreferredClass = preferredClass
            };

            return relic;            
        }
    }
}