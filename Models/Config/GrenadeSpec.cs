namespace Models.Config
{
    public class GrenadeSpec : ItemSpec 
    {
        public string Damage {get; set;}

        public string GrenadeType {get; set;}      

        public bool? IsElemental {get; set;}  
    }
}