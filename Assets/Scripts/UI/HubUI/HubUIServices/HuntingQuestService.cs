using System.Collections.Generic;


namespace BeastHunterHubUI
{
    public class HuntingQuestService
    {
        HubUIData _hubUIData;

        public HuntingQuestService()
        {
            _hubUIData = BeastHunter.Data.HubUIData;
        }


        public List<HuntingQuestModel> GetRandomQuests()
        {
            List<HuntingQuestModel> list = new List<HuntingQuestModel>();

            List<int> questIndexes = new List<int>();
            for (int i = 0; i < _hubUIData.HuntingQuestsDataPool.Length; i++)
            {
                questIndexes.Add(i);
            }

            List<int> bossIndexes = new List<int>();
            for (int i = 0; i < _hubUIData.BossesDataPool.Length; i++)
            {
                bossIndexes.Add(i);
            }

            for (int i = 0; i < _hubUIData.QuestRoomData.HuntingQuestAmount; i++)
            {
                int questIndex = questIndexes[UnityEngine.Random.Range(0, questIndexes.Count)];
                int bossIndex = bossIndexes[UnityEngine.Random.Range(0, bossIndexes.Count)];

                list.Add(new HuntingQuestModel(_hubUIData.HuntingQuestsDataPool[questIndex], _hubUIData.BossesDataPool[bossIndex]));

                questIndexes.Remove(questIndex);
                bossIndexes.Remove(bossIndex);
            }

            return list;
        }
    }
}
