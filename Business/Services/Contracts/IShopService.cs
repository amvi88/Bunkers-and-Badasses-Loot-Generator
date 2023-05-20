using Models.Builder;

namespace Business.Services
{
    public interface IShopService
    {
        IEnumerable<Item> GenerateInventory(ShopServiceParameters parameters);
    }
}