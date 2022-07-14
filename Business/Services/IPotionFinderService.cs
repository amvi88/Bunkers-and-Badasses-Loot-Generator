using Business.Models.Builder;
using Business.Models.Config;

namespace Business.Services
{
    public interface IPotionFinderService
    {
        Potion GetPotion(ChestPotionSpec potionSpec);
    }
}