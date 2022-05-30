namespace Business.Models
{
    public class Potion : Item
    {        
        public string Name {get; set;}

        public string Effect {get; set;}

        public int Cost {get; set;}

        public bool TinyTinasPotion {get; set;}
    }
}