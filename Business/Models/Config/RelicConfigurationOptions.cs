using Business.Models.Common;

namespace Business.Models.Config
{
    public class RelicConfigurationOptions
    {
        public string[] Classes {get; set;}
        public RelicSpec[] RelicSpecs { get; set; }
    }
}