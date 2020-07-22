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
        public GameObject ItemObjectList;
        public static List<GameObject> NpcList = new List<GameObject>();
        public List<ItemInfo> ItemList = new List<ItemInfo>();
        public List<GameObject> ItemListObj = new List<GameObject>();

        public GameObject Npc;
        public GameObject Item;

        public Text NpcName;
        public int NpcId;
        public Image NpcAvatar;
        public Image StartQuest;
        public Image Task;
        public Image FinishQuest;

        public Text ItemName;
        public int ItemId;
        public Image ItemAvatar;

        public GameObject toTolk;
        public Text ToSearch;

        public int SelectedNpcId;
        

        public void Awake()
        {
            InteractiveField = gameObject.transform.GetChild(0).gameObject;
            PlaceName = gameObject.transform.Find("Background/TitleImage/TitleText").GetComponent<Text>();
            NpcObjectList = gameObject.transform.Find("Background/npcList").gameObject;
            ItemObjectList = gameObject.transform.Find("Background/itemList").gameObject;
            toTolk = gameObject.transform.Find("Background/toTalk").gameObject;
            ToSearch = gameObject.transform.Find("Background/toSearch").GetComponentInChildren<Text>();

            NpcName = Npc.transform.Find("currentNpcName/Text").GetComponent<Text>();
            NpcAvatar = Npc.transform.Find("avatar").GetComponent<Image>();
            StartQuest = Npc.transform.Find("StartQuest").GetComponent<Image>();
            Task = Npc.transform.Find("TaskQuest").GetComponent<Image>();
            FinishQuest = Npc.transform.Find("FinishQuest").GetComponent<Image>(); ;

            ItemName = Item.transform.Find("currentItemName/Text").GetComponent<Text>();
            ItemAvatar = Item.transform.Find("itemAvatar").GetComponent<Image>();

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
            foreach (var item in ItemListObj)
            {
                Destroy(item);
            }
            NpcList.Clear();
            ItemList.Clear();
            InteractiveField.SetActive(true);
            PlaceName.text = place.PlaceInfo.PlaceName;
            PlaceId = place.PlaceInfo.PlaceId;
            //if (place.PlaceInfo.PlaceId == 1)
            //{
            //    ToSearch.text = "To come in";
            //}
            //else
            //{
            //    ToSearch.text = "To search";
            //}
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
                ItemName.text = place.itemList[0].ItemName;
                ItemAvatar.sprite = place.itemList[0].ItemImage;
                var tempItem = Instantiate(Item, ItemObjectList.transform.position, ItemObjectList.transform.rotation, ItemObjectList.transform);
                ItemListObj.Add(tempItem);
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
