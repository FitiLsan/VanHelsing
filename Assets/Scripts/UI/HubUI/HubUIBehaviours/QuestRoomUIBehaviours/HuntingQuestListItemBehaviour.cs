using System;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public class HuntingQuestListItemBehaviour : MonoBehaviour
    {
        public Action<HuntingQuestModel> OnClickButtonHandler { get; set; }


        public void FillInfo(HuntingQuestModel quest)
        {
            GetComponentInChildren<Text>().text = quest.Title;
            GetComponent<Button>().onClick.AddListener(() => OnClickButton(quest));
        }


        private void OnClickButton(HuntingQuestModel quest)
        {
            OnClickButtonHandler?.Invoke(quest);
        }
    }
}
