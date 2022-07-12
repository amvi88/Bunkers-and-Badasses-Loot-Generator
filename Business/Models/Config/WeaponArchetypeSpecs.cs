namespace Business.Models.Config
{
    public class WeaponArchetypeSpecs
    {
            public int MinLevel { get; set;}
            public int MaxLevel{ get; set;}
            public string Damage { get; set;}
            public int Range { get; set;}     
            public List<WeaponHits> HitsByAccuracy { get; set;}  
    }

}