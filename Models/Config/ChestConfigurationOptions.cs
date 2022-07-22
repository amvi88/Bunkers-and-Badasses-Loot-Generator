namespace Models.Config
{
    public class ChestConfigurationOptions
    {
        public ChestSpec[] DiceChests { get; set; } = new ChestSpec[0];

        public ChestSpec[] UnassumingChests { get; set; } = new ChestSpec[0];

        public ChestSpec[] CacheRolls { get; set; } = new ChestSpec[0];

        public ChestSpec[][] EnemyDrops { get; set; }
    }
}