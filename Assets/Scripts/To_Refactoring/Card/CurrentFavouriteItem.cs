using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentFavouriteItem : MonoBehaviour, IDropHandler
{
    private int itemNum;

    public Item currentFavouriteItem
    {
        get { return CardItemManager.instanceInventory.favourite.favouriteItem[ItemNum]; }
        set { CardItemManager.instanceInventory.favourite.favouriteItem[ItemNum] = value; }
    }

    public int ItemNum
    {
        get { return itemNum; }
        set { itemNum = value; }
    }


    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = Drag.isDragingObject;

        if (dragedObject == null)
        {
            return;
        }



        CurrentFavouriteItem dragedFavouriteItem = dragedObject.GetComponent<CurrentFavouriteItem>();
        CurrentFavouriteoneItem dragedFavouriteoneItem = dragedObject.GetComponent<CurrentFavouriteoneItem>();
        CurrentItem dragedCurrentItem = dragedObject.GetComponent<CurrentItem>();

        Drag drag = Drag.isDragingObject.GetComponent<Drag>();
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (dragedObject.GetComponent<CurrentItem>())
            {
                if (dragedCurrentItem.currentInventoryItem.id == GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id)
                {
                    if (dragedCurrentItem.currentInventoryItem.isStackable)
                    {
                        int count = dragedCurrentItem.currentInventoryItem.countItem;
                        GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem += count;
                        //  dragedCurrentItem.currentInventoryItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentFavouriteItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedCurrentItem.currentInventoryItem;
                    dragedCurrentItem.currentInventoryItem = currentFavouriteItem;
                }
            }

            if (dragedObject.GetComponent<CurrentFavouriteItem>())
            {
                if (dragedFavouriteItem.currentFavouriteItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
                {
                    if (dragedFavouriteItem.currentFavouriteItem.isStackable)
                    {
                        int count = dragedFavouriteItem.currentFavouriteItem.countItem;
                        GetComponent<CurrentItem>().currentInventoryItem.countItem += count;
                        //dragedFavouriteItem.currentFavouriteItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
                    GetComponent<CurrentItem>().currentInventoryItem = dragedFavouriteItem.currentFavouriteItem;
                    dragedFavouriteItem.currentFavouriteItem = currentItem;
                }
            }

            if (dragedObject.GetComponent<CurrentFavouriteoneItem>())
            {
                if (dragedFavouriteoneItem.currentFavouriteoneItem.id == GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id)
                {
                    if (dragedFavouriteoneItem.currentFavouriteoneItem.isStackable)
                    {
                        int count = dragedFavouriteoneItem.currentFavouriteoneItem.countItem;
                        GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem += count;
                        //  dragedCurrentItem.currentInventoryItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentFavouriteItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedFavouriteoneItem.currentFavouriteoneItem;
                    dragedFavouriteoneItem.currentFavouriteoneItem = currentFavouriteItem;
                }
            }

           




            CardItemManager.instanceInventory.DisplayItems();
          
            if (CardItemManager.instanceInventory)
                CardItemManager.instanceInventory.favourite.DisplayItems();


        }


    } 

   
}

