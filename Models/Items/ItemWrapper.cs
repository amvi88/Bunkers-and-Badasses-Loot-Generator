namespace Models.Builder
{
    public class ItemWrapper<T> where T: Item
    {
        public DiceRoll[] DiceRolls {get; set;} = new DiceRoll[0];

        public T Item {get; set;}
    }
}
