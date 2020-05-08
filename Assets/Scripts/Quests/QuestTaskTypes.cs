using System;
using System.Runtime.Serialization;


namespace BeastHunter
{
    [Serializable]
    public enum QuestTaskTypes
    {
        [EnumMember] None            = 0,
        [EnumMember] KillNpc         = 1,
        [EnumMember] CollectItem     = 2,
        [EnumMember] TalkWithNpc     = 3,
        [EnumMember] UseObject       = 4,
        [EnumMember] FindLocation    = 5,
        [EnumMember] KillEnemyFamily = 6,
        [EnumMember] UseItem         = 7,
		[EnumMember] AnswerSelect    = 8 
    }
}