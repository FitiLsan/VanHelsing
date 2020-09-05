
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;




public class CardItemManager : MonoBehaviour
{

    public static CardItemManager Instance;
    public List<CardItem> item;
    public List<CardItem> Card;
    public List<CardItem> Ert;
    //public List<CardItem> CardJ;
    //public Sprite[] CardItemSprites;
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
    [SerializeField]
    public GameObject CardContainer1;
    [SerializeField]
    public GameObject CardContainer2;




    public void Start()
    {
        item = new List<CardItem>();
        Card = new List<CardItem>();

        for (int i = 0; i < SlotContainer.transform.childCount; i++)   // Создания ечейки
        {
            item.Add(new CardItem());
        };




      
        #region



    }




    #endregion
    void Update()
    {
        Transform ItemDgj = CardContainer2.transform;
        Transform ItemDg = CardContainer1.transform;
        Transform ItemD = CardContainer.transform;
        var H = ItemD.GetComponent<CardItem>();
        var g = ItemDg.GetComponent<CardItem>();
        var s = ItemDgj.GetComponent<CardItem>();

        CardItem(H);
        CardItem(g);
        CardItem(s);



    }

    public void CardItem( CardItem AddCard)
    {

        
       
        Ert = new List<CardItem>();
        Ert.Add(AddCard);
       // Ert.Add(AddCard);


        if (Input.GetKeyDown(takeButton))
        {
         
            for (int i = 0; i < item.Count; i++)
            {
               

                if (item[i].Id == 0)
                {
                    //Card = new List<CardItem>();

                    foreach (var ui in Ert)
                    {

                        item[i]=ui.GetComponent<CardItem>();
                    break;
                    }
                    //[i] = Card.GetComponent<CardItem>();

                    //item.
                    //  item.Add(hj);

                    ////if (item[i].Id != 0)
                    //{



                    // добовления предмета из рекаста или  каким то оброзом из списка 
                    // Transform ItemD = CardContainer.transform;
                    Debug.Log("tyu");



                        DisplayCardItems();
                    break;
                }

              
                //item.Add(ItemD.GetComponent<CardItem>());
                //    item.Add(ItemDg.GetComponent<CardItem>());
                //GenerateCardItem(); 

                //отресовка в слоте
                //break;
                //Destroy(GetComponent<CardItem>().gameObject);


                //}
            }
        }


    } 

        



    
        


    



  

    void DisplayCardItems()
    {

        for (int i = 0; i < item.Count; i++)
        {

            if (item[i].Id != 0)
            {
                Transform Slot = SlotContainer.transform.GetChild(i);
                Transform icon = Slot.GetChild(0);
                Image img = icon.GetComponent<Image>();

                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(item[i].PathIcon);

                
            }
            Debug.Log("q1");

         
        }
       
    }
}


















