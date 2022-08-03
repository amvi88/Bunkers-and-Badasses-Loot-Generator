using Models.Builder;
using Models.Config;

namespace Business.Services
{
    public interface IPotionFinderService
    {
        Potion GetPotion(ChestPotionSpec potionSpec);
    }
}