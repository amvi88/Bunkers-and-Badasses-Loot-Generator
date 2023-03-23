using Models.Common;

namespace Models.Config
{
    public class SpellTypeSpec
    {
        public SpellType SpellType { get; set; }

        public string Effect { get; set; }

        public SpellDamageSpec[] SpellDamageSpecs { get; set; }
    }
}