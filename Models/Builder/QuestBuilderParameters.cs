using System.ComponentModel.DataAnnotations;
using Models.Common;

namespace Models.Builder
{
    public class QuestBuilderParameters
    {
        [Required]
        [Range(1,30)]
        public int PlayerLevel {get; set;} = 1;

        public string Location {get; set;} = "Somewhere";

        public QuestType QuestType  {get; set;} = QuestType.Main;

        public string ImageUrl {get; set;}

        public string QuestGiver {get; set;}

        public string QuestName {get; set;} = "Quest Name";

        public string Summary {get; set;} = "Summary";

        public string ReadAloudText {get; set;}

        public string Difficulty {get; set;} = "Chaos";

        public int XpReward {get; set;} = 0;

        public int GoldReward {get; set;} = 0;
    }
}