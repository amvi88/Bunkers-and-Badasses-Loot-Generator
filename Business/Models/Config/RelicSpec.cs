using Business.Models.Common;

namespace Business.Models.Config
{
    public class RelicSpec
    {
        public string Type { get; set; }

        public string Effect { get; set; }

        public string ClassEffect { get; set; }

        public Rarity Rarity { get; set; }

        public int? MinRoll {get; set;}

        public int? MaxRoll {get; set;}

        public int? Roll {get; set;}
    }
}