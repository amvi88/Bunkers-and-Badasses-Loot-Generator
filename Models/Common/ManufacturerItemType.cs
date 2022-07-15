using System;

namespace Models.Common
{
    [Flags]
    public enum ItemType : short
    {
        Shield = 1,
        Grenade = 2,
        Gun = 4
    }
}