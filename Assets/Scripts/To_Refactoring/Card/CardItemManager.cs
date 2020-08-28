
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;




public class CardItemManager : MonoBehaviour
{

    public static CardItemManager Instance;
    public List<CardItem> item;
    public List<CardItem> Card;
    public Sprite[] CardItemSprites;
    [SerializeField]
    public string nameCardItem;
    [SerializeField]
    public Sprite pathSprite;

    // public CharCard CharCard;
    [SerializeField]
    public string CardItemBadhar;
    [SerializeField]
    public string CardTextGoodhar;
    [SerializeField]
    public Sprite CardImage;
    [SerializeField]
    public Sprite CardBossImage;
    public CardItem[] _InitialcardItems;
    // [HideInInspector]

    public KeyCode takeButton;
    [SerializeField]
    public GameObject SlotContainer;
    [SerializeField]
    public GameObject CardContainer;






    public void Start()
    {
        item = new List<CardItem>();


        for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ечейки
        {
            item.Add(new CardItem());
        }


        #region

      
       
    }
    //GenerateCardItemll();


    //private void Awake()
    //{
    //    if (!Instance)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    GenerateCardItem();



    //}



    //карта

    //void GenerateCardItem()
    //{
    //    Card = new List<CardItem>();

    //    Card.Add(new CardItem(1, "Jacket", CardItemBadhar, CardItemSprites[0]));
    //    Card.Add(new CardItem(2, "Set", "Урон протев ветра", CardItemSprites[1]));
    //    Card.Add(new CardItem(3, "Anfib", "Урон протев ветра", CardItemSprites[2]));
    //    CardItem.Add(new CardItem("Jacket", "Урон протев ветра", CardItemSprites[3]));

    //}


    // GenerateCardItem();

    #endregion
    void Update()
    {

        if (Input.GetKeyDown(takeButton))
        {
            AddCard();
        }


    }

    void AddCard()
    {
           
        
            for (int i = 0; i < item.Count; i++)
        {

            if (item[i].Id != 0)
            {


                item[i] = GetComponent<CardItem>();  // добовления предмета из рекаста или  каким то оброзом из списка 
            
                //GenerateCardItem();
                DisplayCardItems();//отресовка в слоте
                                   break;
                                   //Destroy(GetComponent<CardItem>().gameObject);

                //}


            }
        }
        }

        void DisplayCardItems()
        {

            for (int i = 0; i < item.Count; i++)
            {

                Transform Slot = SlotContainer.transform.GetChild(i);
                Transform icon = Slot.GetChild(0);
                Image img = icon.GetComponent<Image>();
                //if (item[i].Id == 0)
                //{

                    img.enabled = true;
                    img.sprite = Resources.Load<Sprite>(item[i].PathIcon);
                }
                Debug.Log("q1");


            }

}















