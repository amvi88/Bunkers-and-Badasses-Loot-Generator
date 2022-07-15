using Models.Common;

namespace Models.Config
{
    public class RelicConfigurationOptions
    {
        public string[] Classes {get; set;}
        public RelicSpec[] RelicSpecs { get; set; }
    }
}