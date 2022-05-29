using Business.Models;
using Business.Models.Common;

namespace Business.Factories
{
    public interface IGunFactory : IItemFactory<Gun>
    {
        Gun Manufacture(int playerLevel, Rarity? rarity, GunType gunType);
    }
}