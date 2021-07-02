using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "HuntingQuest", menuName = "CreateData/HubUIData/HuntingQuest", order = 0)]
    public class HuntingQuestSO: ScriptableObject
    {
        [SerializeField] private string _title;
        [SerializeField, TextArea(3,10)] private string _description;
        [SerializeField] int _hoursAmountBeforeHunt;


        public string Title => _title;
        public string Description => _description;
        public int HoursAmountBeforeHunt => _hoursAmountBeforeHunt;
    }
}
