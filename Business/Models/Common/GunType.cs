namespace Business.Models.Common
{
    [Flags]
    public enum GunType : short
    {
        CombatRifle = 1,
        Pistol = 2,
        SubmachineGun = 4,
        Shotgun = 8,
        SniperRifle = 16,
        RocketLauncher = 32
    }
}