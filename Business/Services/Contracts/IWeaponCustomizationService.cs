using Models.Common;
using Models.Config;

namespace Business.Services
{
    public interface IWeaponCustomizationService
    {
        Dictionary<string, Prefix> GetPrefixes();
        Dictionary<string, RedText> GetRedTexts();
        Dictionary<GunType, string> GetWeaponArchetypeBonuses();
        List<WeaponHits> GetWeaponArchetypeHits(GunType gunType, int playerLevel);
    }
}