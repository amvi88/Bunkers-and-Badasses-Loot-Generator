using Business.Models.Common;

namespace Business.Models.Builder
{
    public class Potion : Item
    {
        public string Name { get; set; }

        public string Effect { get; set; }

        public int Cost { get; set; }

        public bool IsTinyTinasPotion { get; set; }

        public bool IsElemental { get; set;}

        public Element Element {get; set;}
    }
}