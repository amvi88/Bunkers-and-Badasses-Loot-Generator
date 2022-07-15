using Models.Common;

namespace Models.Config
{
    public class ElementalChance
    {
        public int MinxRoll {get; set;}
        public int MaxRoll {get; set;}

        public Element[] Elements {get; set;}

        public string[] ExtraDamage {get; set;}
    }
}
