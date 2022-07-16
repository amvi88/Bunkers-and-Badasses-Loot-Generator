using Models;
using Models.Builder;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class MoxxTailFactory : IItemFactory<MoxxTail, BaseFactoryParameters>
    {
        private readonly MoxxTailConfigurationOptions _moxxTailConfiguration;

        public MoxxTailFactory(IOptions<MoxxTailConfigurationOptions> moxxTailConfiguration)
        {
            _moxxTailConfiguration = moxxTailConfiguration.Value ?? throw new ArgumentException(nameof(moxxTailConfiguration));
        }

        public ItemWrapper<MoxxTail> Manufacture(BaseFactoryParameters factoryParameters)
        {
            var roll = RandomNumberGenerator.GetInt32(0, _moxxTailConfiguration.MoxxTails.Count());    
            var specs = _moxxTailConfiguration.MoxxTails.ElementAt(roll);

            return new ItemWrapper<MoxxTail>
            {
                Item = new MoxxTail
                {
                    Name  = specs.Name,
                    Effect = specs.Effect
                }
            };
        }
    }    
}