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

        public List<Trauma> GetTrauma(TraumaServiceParameters parameters)
        {
            List<Trauma> traumas = new List<Trauma>();
            if (parameters.TraumaType == TraumaType.Temporary)
            {
                var roll = RandomNumberGenerator.GetInt32(0, _traumaConfiguration.Temporary.Length);
                var traumaSpec = _traumaConfiguration.Temporary[roll];

                traumas.Add(new Trauma
                    {
                        Roll = roll + 1,
                        Effect = traumaSpec.Effect,
                        Name = traumaSpec.Name,
                        TraumaType = TraumaType.Temporary
                    });
                if (!traumaSpec.RerollAsPermanent.GetValueOrDefault())
                {
                    return traumas;
                }
            }

            var permanentRoll = RandomNumberGenerator.GetInt32(0, _traumaConfiguration.Permanent.Length);
            var permanentTraumaSpec = _traumaConfiguration.Permanent[permanentRoll];

            traumas.Add(new Trauma
            {
                Roll = permanentRoll + 1,
                Effect = permanentTraumaSpec.Effect,
                Name = permanentTraumaSpec.Name,
                TraumaType = TraumaType.Permanent
            });

            return traumas;
        }

    }
}