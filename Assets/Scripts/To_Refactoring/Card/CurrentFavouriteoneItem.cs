using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CurrentFavouriteoneItem : MonoBehaviour, IDropHandler
{
    private int itemNum;
    
    public Item currentFavouriteoneItem
    {
        get { return CardItemManager.instanceInventory.favouriteone.favouriteoneItem[ItemNum]; }
        set { CardItemManager.instanceInventory.favouriteone.favouriteoneItem[ItemNum] = value; }
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

        CurrentFavouriteoneItem dragedFavouriteoneItem = dragedObject.GetComponent<CurrentFavouriteoneItem>();
        CurrentFavouriteItem dragedFavouriteItem = dragedObject.GetComponent<CurrentFavouriteItem>();
      
        CurrentItem dragedCurrentItem = dragedObject.GetComponent<CurrentItem>();
      
        Drag drag = Drag.isDragingObject.GetComponent<Drag>();
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (dragedObject.GetComponent<CurrentItem>())
            {
                if (dragedCurrentItem.currentInventoryItem.id == GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.id)
                {
                    if (dragedCurrentItem.currentInventoryItem.isStackable)
                    {
                        int count = dragedCurrentItem.currentInventoryItem.countItem;
                        GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.countItem += count;
               
                    }
                }
                else
                {
                    Item currentFavouriteoneItem = GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem;
                    GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem = dragedCurrentItem.currentInventoryItem;
                    dragedCurrentItem.currentInventoryItem = currentFavouriteoneItem;
                }
            }

         


            if (dragedObject.GetComponent<CurrentFavouriteoneItem>())
            {
                if (dragedFavouriteoneItem.currentFavouriteoneItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
                {
                    if (dragedFavouriteoneItem.currentFavouriteoneItem.isStackable)
                    {
                        int count = dragedFavouriteoneItem.currentFavouriteoneItem.countItem;
                        GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.countItem += count;
                  
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedFavouriteoneItem.currentFavouriteoneItem;
                    dragedFavouriteoneItem.currentFavouriteoneItem = currentItem;
                } 
            }

            if (dragedObject.GetComponent<CurrentFavouriteItem>())
            {
                if (dragedFavouriteItem.currentFavouriteItem.id == GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.id)
                {
                    if (dragedFavouriteItem.currentFavouriteItem.isStackable)
                    {
                        int count = dragedFavouriteItem.currentFavouriteItem.countItem;
                        GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.countItem += count;
                     
                    }
                }
                else
                {
                    Item currentFavouriteoneItem = GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem;
                    GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem = dragedFavouriteItem.currentFavouriteItem;
                    dragedFavouriteItem.currentFavouriteItem = currentFavouriteoneItem;
                }
            }

          
            CardItemManager.instanceInventory.DisplayItems();
          
            if (CardItemManager.instanceInventory)
                CardItemManager.instanceInventory.favouriteone.DisplayItems();
        }

      
    }

}

