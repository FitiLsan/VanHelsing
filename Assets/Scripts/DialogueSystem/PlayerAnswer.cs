using System;


namespace DialogueSystem
{
    [Serializable]
    public sealed class PlayerAnswer
    {
        #region Properties

        public bool IsEnd { get; private set; }
        public bool IsStartQuest { get; private set; }
        public bool IsEndQuest { get; private set; }
        public bool HasTaskQuest { get; private set; }
        public int QuestId { get; private set; }
        public int AnswerId { get; private set; }
        public string Text { get; private set; }
        public int ToNode { get; private set; }

        #endregion


        #region ClassLifeCycles

        public PlayerAnswer(int answerId, string text, int toNode, int end, int isStartQuest, int isEndQuest, int questId, int taskQuest)
        {
            AnswerId = answerId;
            Text = text;
            ToNode = toNode;
            if (end == 0) IsEnd = false;
            if (end == 1) IsEnd = true;
            if (isStartQuest == 0) IsStartQuest = false;
            if (isStartQuest == 1) IsStartQuest = true;
            if (isEndQuest == 0) IsEndQuest = false;
            if (isEndQuest == 1) IsEndQuest = true;
            QuestId = questId;
            if (taskQuest == 0) HasTaskQuest = false;
            if (taskQuest == 1) HasTaskQuest = true;
        }

        #endregion
    }
}