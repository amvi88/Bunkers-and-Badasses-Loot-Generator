using System.Security.Cryptography;
using Business.Models.Builder;
using Business.Models.Common;
using Business.Models.Config;
using Microsoft.Extensions.Options;

namespace Business.Services
{

    public class TraumatizingService : ITraumatizingService
    {
        private readonly TraumasConfigurationOptions _traumaConfiguration;

        public TraumatizingService(IOptions<TraumasConfigurationOptions> traumaConfiguration)
        {
            _traumaConfiguration = traumaConfiguration.Value ?? throw new ArgumentNullException(nameof(traumaConfiguration));
        }

        public Trauma GetTrauma(TraumaServiceParameters parameters)
        {
            if (parameters.TraumaType == TraumaType.Temporary)
            {
                var roll = RandomNumberGenerator.GetInt32(0, _traumaConfiguration.Temporary.Length);
                var traumaSpec = _traumaConfiguration.Temporary[roll];

                if (!traumaSpec.RerollAsPermanent.GetValueOrDefault())
                {
                    return new Trauma
                    {
                        Roll = roll + 1,
                        Effect = traumaSpec.Effect,
                        Name = traumaSpec.Name,
                        TraumaType = TraumaType.Temporary
                    };
                }
            }

            return new Trauma
            {
                TraumaType = TraumaType.Permanent
            };
        }

    }
}