using System.ComponentModel;

namespace Models.Common
{
    [Flags]
    public enum SpellType : int
    {
        [Description("Arc Torrent")]
        ArcTorrent = 1,
        [Description("Sigil")]
        Sigil = 2,
        [Description("Eruption")]
        Eruption = 4,
        [Description("Fireball")]
        Fireball = 8,
        [Description("Sunder")]
        Sunder = 16,
        [Description("Talon")]
        Talon = 32,
        [Description("Hydra")]
        Hydra = 64,
        [Description("Ice Spike")]
        IceSpike = 128,
        [Description("Barrage")]
        Barrage = 256,
        [Description("Calamity")]
        Calamity = 512,
    }
}