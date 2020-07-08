using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class PlaceInteractiveField : MonoBehaviour
    {
        public GameObject InteractiveField;
        public Text PlaceName;
        public int PlaceId;
        public GameObject NpcObjectList;
        public static List<GameObject> NpcList = new List<GameObject>();
        public List<ItemInfo> ItemList = new List<ItemInfo>();

        public GameObject Npc;

        public Text NpcName;
        public int NpcId;
        public Image NpcAvatar;
        public Image StartQuest;
        public Image Task;
        public Image FinishQuest;

        public GameObject toTolk;
        public Text ToSearch;

        public int SelectedNpcId;
        

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
            Task = Npc.transform.Find("TaskQuest").GetComponent<Image>();
            FinishQuest = Npc.transform.Find("FinishQuest").GetComponent<Image>(); ;

            PlaceButtonClick.ClickEvent += OnClick;
            SelectNpcButtonClick.SelectNpcClickEvent += OnNpcSelect;
        }

        private void OnClick(Place place)
        {
            NpcId = 0;
            SelectedNpcId = 0;
            foreach (var npc in NpcList)
            {
                Destroy(npc);
            }
            NpcList.Clear();
            ItemList.Clear();
            InteractiveField.SetActive(true);
            PlaceName.text = place.PlaceInfo.PlaceName;
            PlaceId = place.PlaceInfo.PlaceId;
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
                NpcId = place.npcList[0].NpcId;
                var tempNpc = Instantiate(Npc, NpcObjectList.transform.position, NpcObjectList.transform.rotation, NpcObjectList.transform);
                NpcList.Add(tempNpc);
                
            }
            if (place.itemList.Count != 0)
            {
                ItemList.AddRange(place.itemList);
            }
        }
        private void OnNpcSelect(GameObject npc)
        {
            SelectedNpcId = NpcId;
        }
        
        public static List<GameObject> GetNpcList()
        {
            return NpcList;
        }
    }
}
