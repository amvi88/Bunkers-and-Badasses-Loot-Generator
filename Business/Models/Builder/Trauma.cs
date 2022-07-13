using Business.Models.Common;

namespace Business.Models.Builder
{
    public class Trauma 
    {

        public int Roll  {get; set;}
        public string Name {get; set;}
        public string Effect {get; set;}
        public TraumaType TraumaType {get; set;}

    }
}