using Microsoft.Extensions.Options;
using Models.Builder;
using Models.Common;
using Models.Config;

namespace Business.Services
{

    public class WeaponCustomizationService : IWeaponCustomizationService
    {
        private readonly WeaponCustomizationOptions _weaponCustomizationOptions;
        private readonly WeaponArchetypesOptions _weaponArchetypesOptions;

        public WeaponCustomizationService(IOptions<WeaponCustomizationOptions> weaponCustomizationOptions, IOptions<WeaponArchetypesOptions> weaponArchetypesOptions)
        {
            _weaponCustomizationOptions = weaponCustomizationOptions.Value ?? throw new ArgumentNullException(nameof(weaponCustomizationOptions));
            _weaponArchetypesOptions = weaponArchetypesOptions.Value ?? throw new ArgumentNullException(nameof(weaponArchetypesOptions));
        }

        public Dictionary<string, Prefix> GetPrefixes()
        {
            return _weaponCustomizationOptions.Prefixes.ToDictionary(
                p => p.Name,
                p => p
            );
        }

        public Dictionary<string, RedText> GetRedTexts()
        {
            return _weaponCustomizationOptions.RedText.ToDictionary(
                p => p.Name,
                p => p
            );
        }

        public Dictionary<GunType, string> GetWeaponArchetypeBonuses()
        {
            return _weaponArchetypesOptions.Archetypes.ToDictionary(
                a => a.GunType,
                a => a.Bonus
            );
        }

        public List<WeaponHits> GetWeaponArchetypeHits(GunType gunType, int playerLevel)
        {
            return _weaponArchetypesOptions.Archetypes
            .First(a => a.GunType == gunType)
            .WeaponSpecs.First(x => x.MinLevel <= playerLevel && playerLevel <= x.MaxLevel)
            .HitsByAccuracy;
        }
    }
}