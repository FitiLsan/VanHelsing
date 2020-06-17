using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class PlaceInteractiveField : MonoBehaviour
    {
        public GameObject InteractiveField;
        public Text PlaceName;
        public GameObject NpcObjectList;
        public List<GameObject> NpcList = new List<GameObject>();
        public GameObject Npc;

        public Text NpcName;
        public Image NpcAvatar;
        public Image StartQuest;
        public Image Task;
        public Image FinishQuest;

        public GameObject toTolk;
        public Text ToSearch;

        public GameObject SelectedNpc;

        public void Awake()
        {
            InteractiveField = gameObject.transform.GetChild(0).gameObject;
            PlaceName = gameObject.transform.Find("Background/TitleImage/TitleText").GetComponent<Text>();
            NpcObjectList = gameObject.transform.Find("Background/npcList").gameObject;
            toTolk = gameObject.transform.Find("Background/toTalk").gameObject;
            ToSearch = gameObject.transform.Find("Background/toSearch").GetComponentInChildren<Text>();

            NpcName = Npc.transform.Find("currentNpcName/Text").GetComponent<Text>();
            NpcAvatar = Npc.transform.Find("avatar").GetComponent<Image>();
            StartQuest = Npc.transform.Find("StartQuest").GetComponent<Image>();
            Task = Npc.transform.Find("Task").GetComponent<Image>();
            FinishQuest = Npc.transform.Find("FinishQuest").GetComponent<Image>(); ;

            PlaceButtonClick.ClickEvent += OnClick;
            SelectNpcButtonClick.SelectNpcClickEvent += OnNpcSelect;
        }

        private void OnClick(Place place)
        {
            foreach (var npc in NpcList)
            {
                Destroy(npc);
            }
            NpcList.Clear();
            InteractiveField.SetActive(true);
            PlaceName.text = place.PlaceInfo.PlaceName;
            if (place.PlaceInfo.PlaceId == 1)
            {
                ToSearch.text = "To come in";
            }
            else
            {
                ToSearch.text = "To search";
            }
            if (place.npcList.Count != 0)
            {
                NpcName.text = place.npcList[0].NpcName;
                NpcAvatar.sprite = place.npcList[0].NpcImage;
                var tempNpc = Instantiate(Npc, NpcObjectList.transform.position, NpcObjectList.transform.rotation, NpcObjectList.transform);
                NpcList.Add(tempNpc);
            }
        }
        private void OnNpcSelect(GameObject npc)
        {
            SelectedNpc = npc;
        }
    }
}
