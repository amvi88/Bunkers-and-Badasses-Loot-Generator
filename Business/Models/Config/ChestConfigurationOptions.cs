namespace Business.Models.Config
{
    public class ChestConfigurationOptions
    {
        public ChestSpec[] DiceChests { get; set; } = new ChestSpec[0];

        public ChestSpec[] UnassumingChests { get; set; } = new ChestSpec[0];

        public ChestSpec[] Caches { get; set; } = new ChestSpec[0];
    }
}