using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Favouriteone : MonoBehaviour
{
	[HideInInspector]
    public List<Item> favouriteoneItem;

    void Start()
    {
        InitFavouriteone();
        DisplayItems();
        NumItems();
    }

    void InitFavouriteone()
    {
        favouriteoneItem = new List<Item>();
        for (int i = 0; i < transform.childCount; i++)
        {
            favouriteoneItem.Add(new Item());
        }
    }

    void NumItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cell = transform.GetChild(i);
            cell.GetComponent<CurrentFavouriteoneItem>().ItemNum = i;
        }
    }

    public void DisplayItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Item currentItem = favouriteoneItem[i];
            Transform cell = transform.GetChild(i);

            Image icon = cell.transform.GetChild(0).GetComponent<Image>();
            //Text count = icon.transform.GetChild(0).GetComponent<Text>();

            if (currentItem.id != 0)
            {
                icon.enabled = true;
                Sprite itemIcon = currentItem.Icon;
                icon.sprite = itemIcon;
                //count.text = null;

                if (currentItem.isStackable)
                {
                    //if (currentItem.countItem > 1)
                    //    count.text = currentItem.countItem.ToString();
                    //else
                    //    count.text = null;
                }
            }
            else
            {
                icon.enabled = false;
                icon.sprite = null;
                //count.text = null;
            }
        }
    }

    public void RemoveItem(int numItem)
    {
        if (favouriteoneItem[numItem].countItem > 1)
            favouriteoneItem[numItem].countItem--;
        else
            //favouriteItem[numItem] = CardItemManager.instanceInventory/*EmptySlot()*/

        DisplayItems();
    }

    public bool isExistItem(int id)
    {
        for (int i = 0; i < favouriteoneItem.Count; i++)
        {
            if (favouriteoneItem[i].id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItemID(int id)
    {
        for (int i = 0; i < favouriteoneItem.Count; i++)
        {
            if (favouriteoneItem[i].id == id)
            {
                if (favouriteoneItem[i].countItem > 1)
                    favouriteoneItem[i].countItem--;
                else
                  //  favouriteItem[i] = CardItemManager.instanceInventory.EmptySlot();
                DisplayItems();
            }
        }
    }

}
