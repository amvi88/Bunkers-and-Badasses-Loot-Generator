using Business.Models.Builder;

namespace Business.Services
{
    public interface IChestService<T> where T: BaseFactoryParameters
    {
        Chest OpenChest(T parameters);
    }
}