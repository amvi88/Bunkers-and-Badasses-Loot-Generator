using Models.Common;

namespace Models.Config
{
    public class ChestSpec
    {
        public bool IsMimic  { get; set;}
        
        public int Roll { get; set;}

        public int Gold {get; set;}

        public int AmountOfGrenades {get; set;}

        public int AmountOfGrenadeMods {get; set;}

        public int AmountOfShieldMods {get; set;}

        public ChestWeaponSpec[] WeaponSpecs {get; set;} = new ChestWeaponSpec[0];

        public ChestRelicSpec[] RelicSpecs {get; set;} = new ChestRelicSpec[0];

        public ChestPotionSpec[] PotionSpecs {get; set;} = new ChestPotionSpec[0];
    }

}