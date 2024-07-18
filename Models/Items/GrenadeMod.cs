using Models.Common;

namespace Models.Builder
{
    public class GrenadeMod : Item
    {
        public int Level { get; set; }

        public string Damage { get; set; }

        public int[] AmountOfDice => Damage.Split(' ','+',',').Select(x => int.Parse(x.Substring(0, 1))).ToArray();

        public string[] DiceType => Damage.Split(' ', '+', ',').Select(x => x.Substring(1)).ToArray();

        public string GrenadeType { get; set; }

        public string Effect { get; set; }

        public Element Element { get; set; }

        public Rarity Rarity { get; set; } = Rarity.Common;

        public RedText RedText { get; set; }
    }
}
