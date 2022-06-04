using Business.Models;
using Business.Models.Common;

namespace Business.Factories
{
    public interface IRelicFactory
    {
        Relic Dig(Rarity? rarity);
    }
}