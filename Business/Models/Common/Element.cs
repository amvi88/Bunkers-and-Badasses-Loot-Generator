namespace Business.Models.Common
{
    [Flags]
    public enum Element : short
    {
        None = 0,
        Incendiary = 1,
        Shock = 2,
        Explosive = 4,
        Corrosive = 8,
        Radiation = 16,
        Cryo = 32
    }
}