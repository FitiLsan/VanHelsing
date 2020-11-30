using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;



public class CurrentItem : MonoBehaviour,IDropHandler
{
    public int itemNum;
    public int index;
   // CardItemManager cardItemManager;
   // GameObject inventoryObj;
    public Item currentInventoryItem
    {
        get { return CardItemManager.instanceInventory.item[ItemNum]; }
        set { CardItemManager.instanceInventory.item[ItemNum] = value; }
    }

    public int ItemNum
    {
        get { return itemNum; }
        set { itemNum = value; }
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
        CurrentItem currentItem1 = dragedObject.GetComponent<CurrentItem>();



        Drag drag = Drag.isDragingObject.GetComponent<Drag>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (dragedObject.GetComponent<CurrentItem>())
            {
                if (currentItem1.currentInventoryItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
                {
                    if (currentItem1.currentInventoryItem.isStackable)
                    {
                        int count = currentItem1.currentInventoryItem.countItem;
                        GetComponent<CurrentItem>().currentInventoryItem.countItem += count;
               
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
                    GetComponent<CurrentItem>().currentInventoryItem = currentItem1.currentInventoryItem;
                    currentItem1.currentInventoryItem = currentItem;
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
                if (dragedFavouriteoneItem.currentFavouriteoneItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
                {
                    if (dragedFavouriteoneItem.currentFavouriteoneItem.isStackable)
                    {
                        int count = dragedFavouriteoneItem.currentFavouriteoneItem.countItem;
                        GetComponent<CurrentItem>().currentInventoryItem.countItem += count;
                     
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
                    GetComponent<CurrentItem>().currentInventoryItem = dragedFavouriteoneItem.currentFavouriteoneItem;
                    dragedFavouriteoneItem.currentFavouriteoneItem = currentItem;
                }
            }

            CardItemManager.instanceInventory.favourite.DisplayItems();
        
            if (CardItemManager.instanceInventory)
                CardItemManager.instanceInventory.DisplayItems();
        }

     

      

    }
}









