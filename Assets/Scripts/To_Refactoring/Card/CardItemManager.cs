
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class CardItemManager : MonoBehaviour
{
    /// <summary>
    /// класс логики объекта карта Отрисовка в слоте , создания слота добовления карты в него
    /// </summary>

    public Favouriteone favouriteone;
    public static CardItemManager Instance;
    public static CardItemManager instanceInventory;
   // [HideInInspector]
    public List<Item> item;
    public List<Item> Card;
    public List<Item> Ert;
    public List<Item> CardItems;
   
    public Sprite[] CardItemSprites;
    [SerializeField]
    public string nameCardItem;
    [SerializeField]
    public Sprite pathSprite;
    public KeyCode showInventory;

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
       // cellContainer.SetActive(false);

      for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ячейки
        {

            SlotContainer.transform.GetChild(i).GetComponent<CurrentItem>().index=i;
          
        }
        for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ячейки
        {
            item.Add(new Item());
        }   
    }

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
    }
    void Update()
    {

      
       
       // Transform ItemA = CardContainer2.transform;
        Transform ItemB = CardContainer1.transform; //оброщения к обекту карта
        Transform ItemD = CardContainer.transform;
        var CardItemD = ItemD.GetComponent<Item>();
        var CardItemB = ItemB.GetComponent<Item>();
        //var CardItemA = ItemA.GetComponent<Item>();
        //Debug.Log("tdu");

        //  вызов метода
        CardItemAdd(CardItemD); //методы реалезацию  можно разнести по объектом к примеру другой персонаж
        CardItemAdd(CardItemB);
        //CardItemAdd(CardItemA);
    }

    //метод добовления карты
    public void CardItemAdd(Item Add)
    {

        Ert = new List<Item>();
        Ert.Add(Add);
        //Debug.Log("typ");


        if (Input.GetKeyDown(takeButton)) //механика добовления карты может быть не кнопка встреча двух колорайдеров(пример)
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
                    DisplayItems(); //метод отрисвки на экране 
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
                Transform cell = SlotContainer.transform.GetChild(i);

                Transform Icon = cell.GetChild(0);
                Image img = Icon.GetComponent<Image>();


                img.enabled = true;
                img.sprite = item[i].Icon;
    
                }
                    else
                {
                Transform cell = SlotContainer.transform.GetChild(i);
                Transform Icon = cell.GetChild(0);
                Image img = Icon.GetComponent<Image>();
                img.enabled = false;
                img.sprite = null;
                Debug.Log("q1");
            }
            }

        }

    public Item EmptySlot()
    {
        return new Item();
    }


}

































