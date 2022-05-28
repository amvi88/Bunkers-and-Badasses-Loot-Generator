using Business.Models;
using Business.Models.Common;

namespace Business.Factories
{
    public interface IItemFactory<T> where T: Item
    {
        public T Manufacture(int playerLevel, Rarity? rarity);
    }

}