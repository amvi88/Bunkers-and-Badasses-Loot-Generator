using Business.Factories;
using Models.Builder;

namespace Business.Services
{
    public class SpellService : ISpellService
    {
        private readonly IItemFactory<Spell, SpellRandomizerFactoryParameters> spellFactory;

        public SpellService(IItemFactory<Spell, SpellRandomizerFactoryParameters> spellFactory)
        {
            this.spellFactory = spellFactory ?? throw new ArgumentNullException(nameof(spellFactory));
        }

        public ItemWrapper<Spell> RandomizeSpell(SpellRandomizerFactoryParameters parameters)
        {
            return spellFactory.Manufacture(parameters);
        }
    }
}