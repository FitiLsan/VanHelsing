using System.Collections.Generic;

namespace Quests
{
    /// <summary>
    ///     This class used for transfer data from data access layer to logic layer and contains only data.
    /// </summary>
    public class QuestDto
    {
        public int Id { get; set; }

        public List<int> RequiredQuests { get; set; } = new List<int>();

        public string Title { get; set; }

        public string Description { get; set; }

        public int ZoneId { get; set; }

        public List<QuestMarkerDto> MapMarkers { get; set; } = new List<QuestMarkerDto>();

        public List<QuestTaskDto> Tasks { get; set; } = new List<QuestTaskDto>();

        public QuestMarkerDto QuestStart { get; set; }

        public QuestMarkerDto QuestEnd { get; set; }

        public int MinLevel { get; set; }

        public int QuestLevel { get; set; }

        public int TimeAllowed { get; set; }

        public int RewardExp { get; set; }

        public int RewardMoney { get; set; }

        public int StartDialogId { get; set; }
        public int EndDialogId { get; set; }

        public List<QuestRewardDto> Rewards { get; set; } = new List<QuestRewardDto>();
    }
}