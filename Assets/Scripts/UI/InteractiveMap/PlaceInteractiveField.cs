using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class PlaceInteractiveField : MonoBehaviour
    {
        public GameObject InteractiveField;

        public GameObject PlaceEntrance;
        public GameObject PlaceInside;
        public GameObject PlaceFoundItems;
        public GameObject PlaceExitButton;
        public GameObject CurrentPositon;
        public Place CurrentPlace;

        public Text PlaceName;
        public int PlaceId;
        public GameObject NpcObjectPrototype;
        public GameObject ItemObjectPrototype;
        public static List<GameObject> NpcList = new List<GameObject>();
        public List<ItemInfo> ItemList = new List<ItemInfo>();
        public List<GameObject> ItemListObj = new List<GameObject>();
        public Image PlaceImg;
        public GameObject Npc;
        public GameObject Item;

        public Text NpcName;
        public NpcIdInfo NpcId;
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
            NpcObjectPrototype = gameObject.transform.Find("Background/PlaceInside/npcList").gameObject;
            ItemObjectPrototype = gameObject.transform.Find("Background/FoundItems/itemList").gameObject;
            //  toTolk = gameObject.transform.Find("Background/toTalk").gameObject;
            //  ToSearch = gameObject.transform.Find("Background/toSearch").GetComponentInChildren<Text>();
            PlaceEntrance = gameObject.transform.Find("Background/PlaceEntrance").gameObject;
            PlaceInside = gameObject.transform.Find("Background/PlaceInside").gameObject;
            PlaceFoundItems = gameObject.transform.Find("Background/FoundItems").gameObject;
            PlaceExitButton = gameObject.transform.Find("Background/toExit").gameObject;
            CurrentPositon = GameObject.Find("currentPosition");
            PlaceImg = gameObject.transform.Find("Background/PlaceEntrance/image").GetComponent<Image>();

            NpcId = Npc.transform.GetComponent<NpcIdInfo>();
            NpcName = Npc.transform.Find("currentNpcName/Text").GetComponent<Text>();
            NpcAvatar = Npc.transform.Find("avatar").GetComponent<Image>();
            StartQuest = Npc.transform.Find("StartQuest").GetComponent<Image>();
            Task = Npc.transform.Find("TaskQuest").GetComponent<Image>();
            FinishQuest = Npc.transform.Find("FinishQuest").GetComponent<Image>(); ;

            ItemName = Item.transform.Find("currentItemName/Text").GetComponent<Text>();
            ItemAvatar = Item.transform.Find("itemAvatar").GetComponent<Image>();


            PlaceButtonClick.ClickEvent += OnClick;
            ToComeInPlace.ToComeInPlaceObjEvent += OnComeIn;
            PlaceExit.PlaceExitEvent += OnExit;
            ToSearchPlace.ToSearchPlaceEvent += OnSearch;
            ConfirmItems.ConfirmItemsEvent += OnConfirmItems;
        }

        public void MovePosition()
        {
            CurrentPositon.transform.SetPositionAndRotation(CurrentPlace.transform.position, CurrentPlace.transform.rotation);
        }

        public void Start()
        {
            InteractiveField.SetActive(false);
            PlaceInside.SetActive(false);
            PlaceFoundItems.SetActive(false);
            PlaceExitButton.SetActive(true);
        }

        private void OnComeIn()
        {
            PlaceInside.SetActive(true);
            PlaceEntrance.SetActive(false);
            PlaceExitButton.SetActive(true);
            MovePosition();
            
        }

        public void OnExit()
        {
            InteractiveField.SetActive(false);
            PlaceEntrance.SetActive(true);
            PlaceInside.SetActive(false);
            PlaceFoundItems.SetActive(false);
            PlaceExitButton.SetActive(false);
        }

        private void OnSearch(List<ItemInfo> itemList)
        {
            PlaceFoundItems.SetActive(true);
        }

        private void OnConfirmItems()
        {
            PlaceFoundItems.SetActive(false);
        }

        private void OnClick(Place place)
        {
            InteractiveField.SetActive(true);
            PlaceEntrance.SetActive(true);
            PlaceInside.SetActive(false);
            NpcId.NpcID = 0;
            SelectedNpcId = 0;
            CurrentPlace = place;
            PlaceImg.sprite = place.PlaceInfo.PlaceImage;
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


            PlaceName.text = place.PlaceInfo.PlaceName;
            PlaceId = place.PlaceInfo.PlaceId;

            if (place.npcList.Count != 0)
            {
                for (int i = 0; i < place.npcList.Count; i++)
                {
                    NpcName.text = place.npcList[i].NpcName;
                    NpcAvatar.sprite = place.npcList[i].NpcImage;
                    NpcId.NpcID = place.npcList[i].NpcId;
                    var tempNpc = Instantiate(Npc, NpcObjectPrototype.transform.position, NpcObjectPrototype.transform.rotation, NpcObjectPrototype.transform);
                    NpcList.Add(tempNpc);
                }

            }
            if (place.itemList.Count != 0)
            {
                for (int i = 0; i < place.itemList.Count; i++)
                {
                    ItemName.text = place.itemList[i].ItemName;
                    ItemAvatar.sprite = place.itemList[i].ItemImage;
                    var tempItem = Instantiate(Item, ItemObjectPrototype.transform.position, ItemObjectPrototype.transform.rotation, ItemObjectPrototype.transform);
                    ItemListObj.Add(tempItem);
                    ItemList.AddRange(place.itemList);
                }
            }
        }

        public static List<GameObject> GetNpcList()
        {
            return NpcList;
        }
    }
}
