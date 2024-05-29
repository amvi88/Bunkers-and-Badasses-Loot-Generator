using Models.Builder;
using Models.Common;
using Models.Config;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Business.Factories
{
    public class SpellFactory : GuildDrivenFactory<Spell, SpellRandomizerFactoryParameters>
    {
        private readonly SpellConfigurationOptions _spellCustomizationOptions;
        private readonly CustomizationOptions _weaponCustomizationOptions;

        public SpellFactory(IOptions<GuildConfigurationOptions> guildOptions, IOptions<CustomizationOptions> weaponCustomizationOptions, IOptions<SpellConfigurationOptions> spellCustomizationOptions)
            : base(guildOptions)
        {
            _spellCustomizationOptions = spellCustomizationOptions.Value ?? throw new ArgumentException(nameof(spellCustomizationOptions));
            _weaponCustomizationOptions = weaponCustomizationOptions.Value ?? throw new ArgumentException(nameof(weaponCustomizationOptions));
        }
       
        public override ItemWrapper<Spell> Manufacture(SpellRandomizerFactoryParameters builderArguments)
        {
            
            var rarity = GetRarity(builderArguments.Rarity);  
            var spellType = GetSpellType();  
            var chosenGuild = GetGuild(builderArguments.Guild, ItemType.Spell, out var roll);
             
            // Create a spell based on Guild specs
            var spellEffect = chosenGuild.SpellEffectSpecs.First(x => x.MinLevel <= builderArguments.PlayerLevel && builderArguments.PlayerLevel <= x.MaxLevel);
            var guildSpellEffect = chosenGuild.SpellSpecs.First(x => x.SpellType == spellType);
            var spellSpec = _spellCustomizationOptions.Spells.First(x => x.SpellType == spellType);
            var spellDamageSpec = spellSpec.SpellDamageSpecs.First(x => x.MinLevel <= builderArguments.PlayerLevel && builderArguments.PlayerLevel <= x.MaxLevel);

            var spell = new Spell
            {
                Level = builderArguments.PlayerLevel,
                Guild =  chosenGuild.Name,
                AlternateGuildName = chosenGuild.AlternateName,                
                Rarity = rarity,
                Damage = spellDamageSpec.Damage,
                Effect = spellEffect.Effect,
                GuildEffect = guildSpellEffect.Effect,
                Name = spellType.ToString(),
                Source = "wonderlands"
            };

            return new ItemWrapper<Spell>
            {                
                Item = spell
            };
        }

        private Rarity GetRarity(Rarity? rarity)
        {
            if (! rarity.HasValue)
            {
                var rowNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomizationOptions.RarityMatrix.Length);
                var colNumber = RandomNumberGenerator.GetInt32(0, _weaponCustomizationOptions.RarityMatrix[0].Length);
                return _weaponCustomizationOptions.RarityMatrix[rowNumber][colNumber];
            }

            return rarity.GetValueOrDefault();
        }

        private SpellType GetSpellType()
        {
            var spellTypeValues = Enum.GetValues(typeof(SpellType));
            return (SpellType)spellTypeValues.GetValue(RandomNumberGenerator.GetInt32(1,spellTypeValues.Length));
        }
    }
}