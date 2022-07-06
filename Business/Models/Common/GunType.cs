using System.ComponentModel;

namespace Business.Models.Common
{
    [Flags]
    public enum GunType : short
    {
        [Description("Combat Rifle")]
        CombatRifle = 1,
        [Description("Pistol")]
        Pistol = 2,
        [Description("Submachine Gun")]
        SubmachineGun = 4,
        [Description("Shotgun")]
        Shotgun = 8,
        [Description("Sniper Rifle")]
        SniperRifle = 16,
        [Description("Rocket Launcher")]
        RocketLauncher = 32
    }
}