namespace Models.Builder
{
    public class ItemWrapper<T> where T: Item
    {
        public int Roll {get; set;}

        public T Item {get; set;}
    }
}
