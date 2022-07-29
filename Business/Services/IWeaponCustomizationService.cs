using Models.Common;

namespace Business.Services
{
    public interface IWeaponCustomizationService
    {
        Dictionary<string, string> GetPrefixes();
        Dictionary<string, string> GetRedText();
        Dictionary<GunType, string> GetWeaponArchetypeBonuses();
    }
}