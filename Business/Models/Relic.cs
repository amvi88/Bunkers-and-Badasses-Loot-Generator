using Business.Models.Common;

namespace Business.Models
{
    public class Relic : Item
    {
        public string Type { get; set; }

        public string Effect { get; set; }

        public string ClassEffect { get; set; }

        public Rarity Rarity { get; set; }

        public string PreferredClass {get; set;}
    }
}
