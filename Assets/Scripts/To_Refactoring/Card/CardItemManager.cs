
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;




public class CardItemManager : MonoBehaviour
{
    //public Favourite favourite;
    public static CardItemManager Instance;
    public static CardItemManager instanceInventory;
    [HideInInspector]
    public List<Item> item;
    public List<Item> Card;
    public List<Item> Ert;
    public List<Item> CardItems;
    //public List<CardItem> CardJ;
    public Sprite[] CardItemSprites;
    [SerializeField]
    public string nameCardItem;
    [SerializeField]
    public Sprite pathSprite;
     //public int index;
    // public CharCard CharCard;
    [SerializeField]
    public string CardItemBadhar;
    [SerializeField]
    public string CardTextGoodhar;
    [SerializeField]
    public Sprite CardImage;
    [SerializeField]
    public Sprite CardBossImage;
    public Item[] _InitialcardItems;
    public Favourite favourite;
    //[HideInInspector]
    [SerializeField]
    public GameObject cellContainer;

    [SerializeField]
    public KeyCode takeButton;
    public GameObject SlotContainer;
    public GameObject dragPrefab;
    public GameObject database;
    public GameObject CardContainer;
    [SerializeField]
    public GameObject CardContainer1;
    [SerializeField]
    public GameObject CardContainer2;
    [SerializeField]
    public GameObject currentIcon;
 


    public void Start()
    {
       
        item = new List<Item>();
        Card = new List<Item>();
        //SlotContainer.SetActive(false);
        for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ечейки
        {

            SlotContainer.transform.GetChild(i).GetComponent<CurrentItem>().index = i;
            //cellContainer.transform.GetChild(i).GetComponent<CurrentItem>();
        }


        for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ечейки
        {
            item.Add(new Item());
        }

        RenameCells();
        RenameIcons();
    }
            public Item EmptySlot()
            {
                return new Item();
            }

    public int EmptySlotID()
    {
        return 0;
    }

    //public void AddUnStackableItem(Item currentItem)
    //{
    //    for (int i = 0; i < item.Count; i++)
    //    {
    //        if (item[i].id == EmptySlotID())
    //        {
    //            item[i] = currentItem;
    //            item[i].countItem = 1;
    //            DisplayItems();
    //            //Destroy(currentItem.gameObject);
    //            break;
    //        }
    //    }
    //}
    //void AddStackableItem(Item currentItem)
    //{
    //    for (int i = 0; i < item.Count; i++)
    //    {
    //        if (item[i].id == currentItem.id)
    //        {
    //            item[i].countItem++;
    //            DisplayItems();
    //            //Destroy(currentItem.gameObject);
    //            return;
    //        }
    //    }
    //    AddUnStackableItem(currentItem);
    //}

    //public void AddItem(Item currentItem)
    //{
    //    if (currentItem.isStackable)
    //        AddStackableItem(currentItem);
    //    else
    //        AddUnStackableItem(currentItem);
    //}

   
    private void Awake()
    {
        instanceInventory = this;
        if (!Instance)
            Instance = this;


        else 
        {
            Destroy(gameObject);
           
           
            return;
        }
      
        //GenerateItems();
     
    }

    //private void GenerateItems()
    //{

    //        CardItems = new List<Item>();
    //        CardItems.Add(new Item("One", CardItemSprites[0]));
    //        CardItems.Add(new Item("Two", CardItemSprites[1]));
    //        CardItems.Add(new Item("frie", CardItemSprites[2]));
    //}

  
   





    void Update()
    {
        Transform ItemDgj = CardContainer2.transform;
        Transform ItemDg = CardContainer1.transform;
        Transform ItemD = CardContainer.transform;
        var H = ItemD.GetComponent<Item>();
        var g = ItemDg.GetComponent<Item>();
        var s = ItemDgj.GetComponent<Item>();

        Debug.Log("tdu");
      //  вызов метода
        CardItemAdd(H);
        CardItemAdd(g);
        CardItemAdd(s);


    }


    //метод
    public void CardItemAdd(Item Add)
    {



        Ert = new List<Item>();
        Ert.Add(Add);
        //Debug.Log("typ");


        if (Input.GetKeyDown(takeButton))
        {

            for (int i = 0; i < item.Count; i++)
            {


                if (item[i].id == 0)
                {
                    Debug.Log("t4u");

                    foreach (var ui in Ert)
                    {

                        item[i] = ui.GetComponent<Item>();
                        break;
                    }

                    //Debug.Log("tyu");
                    DisplayItems();
                    break;

                }


            }







        }


    }
    //отображения в слоте
    public  void DisplayItems()
    {

        for (int i = 0; i < item.Count; i++)
        {

            if (item[i].id != 0)
            {
                Transform Cell = SlotContainer.transform.GetChild(i);
             
                Transform Icon = Cell.GetChild(0);
                Image img = Icon.GetComponent<Image>();

                img.enabled = true;
                img.sprite = item[i].Icon;
              



            }
            else

            {
                Transform Cell = SlotContainer.transform.GetChild(i);
                
                Transform Icon = Cell.GetChild(0);
                Image img = Icon.GetComponent<Image>();
                img.enabled = false;
                img.sprite = null;
                Debug.Log("q1");
            }
        }

    }
    void CheckEmptyItem(Item item)
    {
        if (item.id == EmptySlotID())
        {
            return;
        }
    }
    void RenameCells()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            Transform cell = SlotContainer.transform.GetChild(i);
            cell.name = "Cell " + i.ToString();
        }
    }

    void RenameIcons()
    {
        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
            Transform cell = SlotContainer.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            icon.name = "Icon " + i.ToString();
        }
    }


    public void RemoveItem(int numItem)
    {
        if (item[numItem].countItem > 1)
        {
            item[numItem].countItem--;
        }
        else
        {
            item[numItem] = EmptySlot();
        }
        DisplayItems();
    }

    public void DropedItem(int id)
    {
        for (int i = 0; i < database.transform.childCount; i++)
        {
            Item item = database.transform.GetChild(i).GetComponent<Item>();
            if (item != null)
            {
                if (item.id == id)
                {
                    GameObject obj = Instantiate(item.gameObject);
                    obj.transform.position = Camera.main.transform.position + transform.forward;
                }
            }
        }
    }
    public bool isExistItem(int id)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItemID(int id)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == id)
            {
                if (item[i].countItem > 1)
                {
                    item[i].countItem--;
                }
                else
                {
                    item[i] = EmptySlot();
                }
                DisplayItems();
            }
        }
    }

}

































