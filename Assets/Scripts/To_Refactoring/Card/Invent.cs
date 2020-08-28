using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Invent : MonoBehaviour
{
    //public static Invent Instance;
    public List<CardItem> item;
    //public Sprite[] CardItamSprites;
    [SerializeField]
    public GameObject SlotContainer;
    [SerializeField]
    public GameObject CardItemContainer;
    public KeyCode takeButton;









    void Start()
    {
        //parametersManager = GetComponent<ParametersManager>();

        //InitInventory();
        item = new List<CardItem>();

        for (int i = 0; i < SlotContainer.transform.childCount; i++)
        {
             item.Add(new CardItem());
        }
        //NumItems();

        //RenameCells();
        //RenameIcons();
        //HidePanel();
    }

    //void InitInventory()
    //{


    //    }
    //}



    public void Update()
    {

        var card = GetComponent<CardItem>();
        item.Add(card);

        if (Input.GetKeyDown(takeButton))
        {



            for (int i = 0; i < item.Count; i++)
            {



                //if (item[i].Id == 0)
                //{

                    item[i] = GetComponent<CardItem>();
                    //item.Add(GetComponent<CardItem>());
                    DisplayCardItem();




                //}



            }
        }
    }



    void DisplayCardItem()
    {

        for (int i = 0; i < item.Count; i++)
        {

            Transform Slot = SlotContainer.transform.GetChild(i);
            Transform icon = Slot.GetChild(0);
            Image img = icon.GetComponent<Image>();
            //if (item[i].Id != 0)
            //{

                img.enabled = true;
                img.sprite = Resources.Load<Sprite>(item[i].PathIcon);
            //}
            Debug.Log("q1");


        }

    }

}











