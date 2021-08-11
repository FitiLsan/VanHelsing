using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Favourite : MonoBehaviour
{
	[HideInInspector]
    public List<Item> favouriteItem;

    void Start()
    {
        InitFavourite();
        DisplayItems();
        NumItems();
    }

    void InitFavourite()
    {
        favouriteItem = new List<Item>();
        for (int i = 0; i < transform.childCount; i++)
        {
            favouriteItem.Add(new Item());
        }
    }

    void NumItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cell = transform.GetChild(i);
            cell.GetComponent<CurrentFavouriteItem>().ItemNum = i;
        }
    }

    public void DisplayItems()
    {
       
            for (int i = 0; i < transform.childCount; i++)
        {
          
                Item currentItem = favouriteItem[i];
            Transform cell = transform.GetChild(i);

            Image icon = cell.transform.GetChild(0).GetComponent<Image>();
            Text name = cell.transform.GetChild(1).GetComponent<Text>();
            Text good = cell.transform.GetChild(2).GetComponent<Text>();
            Text bad = cell.transform.GetChild(3).GetComponent<Text>();
            //Text count = icon.transform.GetChild(0).GetComponent<Text>();

            {
                


                if (currentItem.id != 0)
                {
                    icon.enabled = true;
                    Sprite itemIcon = currentItem.Icon;
                    icon.sprite = itemIcon;
                    //count.text = null;
                    name.enabled = true;
                    Text nam = name.GetComponent<Text>();
                    nam.enabled = true;
                    nam.text = favouriteItem[i].nameItem;

                    good.enabled = true;
                    Text god = good.GetComponent<Text>();
                    god.enabled = true;
                    god.text = favouriteItem[i].descriptiongoodItem;

                    bad.enabled = true;
                    Text bads = bad.GetComponent<Text>();
                    bads.enabled = true;
                    bads.text = favouriteItem[i].descriptionbadItem;
                    //count.text = null;

                    if (currentItem.isStackable)
                    {
                      
                    }

                }
                else
                {
                    icon.enabled = false;
                    icon.sprite = null;
                   //name.text = null;
                    //count.text = null;
                }
            }
        }
    }

    public void RemoveItem(int numItem)
    {
        if (favouriteItem[numItem].countItem > 1)
            favouriteItem[numItem].countItem--;
        else
           

        DisplayItems();
    }

    public bool isExistItem(int id)
    {
        for (int i = 0; i < favouriteItem.Count; i++)
        {
            if (favouriteItem[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItemID(int id)
    {
        for (int i = 0; i < favouriteItem.Count; i++)
        {
            if (favouriteItem[i].id == id)
            {
                if (favouriteItem[i].countItem > 1)
                    favouriteItem[i].countItem--;
                else
                  //  favouriteItem[i] = CardItemManager.instanceInventory.EmptySlot();
                DisplayItems();
            }
        }
    }
}
