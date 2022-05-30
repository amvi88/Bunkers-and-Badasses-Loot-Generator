using Business.Models;
using Business.Models.Common;

namespace Business.Factories
{
    public interface IPotionFactory
    {
        Potion Brew();
    }
}