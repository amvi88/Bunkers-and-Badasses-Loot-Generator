using System;

namespace Business.Models.Common
{
    [Flags]
    public enum ManufacturerItemType : short
    {
        Shield = 1,
        Grenade = 2,
        Gun = 4
    }
}