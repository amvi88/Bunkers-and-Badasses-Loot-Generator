using System;

namespace Business.Models.Common
{
    [Flags]
    public enum ManufacturerItemType : short
    {
        Shield = 1,
        Grenade = 2,
        CombatRifle = 4,
        Pistol = 8,
        SubmachineGun = 16,
        Shotgun = 32,
        SniperRifle = 64,
        RocketLauncher = 128
    }
}