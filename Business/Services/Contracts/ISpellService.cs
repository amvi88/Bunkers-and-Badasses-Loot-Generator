using Models.Builder;

namespace Business.Services
{
    public interface ISpellService
    {
        ItemWrapper<Spell> RandomizeSpell(SpellRandomizerFactoryParameters parameters);
    }
}