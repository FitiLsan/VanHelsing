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
        //if (Input.GetButtonDown((ItemNum+1).ToString()))
        //{
        //    if (currentFavouriteItem.OnItemUse!=null)
        //    {
        //        currentFavouriteItem.OnItemUse.Invoke();
        //    }
        //    if (currentFavouriteItem.isRemovable)
        //    {
        //        CardItemManager.instanceInventory.favourite.RemoveItem(ItemNum);
        //    }
        //}
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = Drag.isDragingObject;

        if (dragedObject == null)
        {
            return;
        }

        Drag drag = Drag.isDragingObject.GetComponent<Drag>();

        CurrentFavouriteItem dragedFavouriteItem = dragedObject.GetComponent<CurrentFavouriteItem>();
        CurrentItem dragedCurrentItem = dragedObject.GetComponent<CurrentItem>();
        //CurrentStorageItem dragedStorageItem = dragedObject.GetComponent<CurrentStorageItem>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (dragedObject.GetComponent<CurrentFavouriteItem>())
            {
                if (dragedFavouriteItem.currentFavouriteItem.id == GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id)
                {
                    if (dragedFavouriteItem.currentFavouriteItem.isStackable)
                    {
                        int count = dragedFavouriteItem.currentFavouriteItem.countItem;
                        GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem += count;
                        dragedFavouriteItem.currentFavouriteItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedFavouriteItem.currentFavouriteItem;
                    dragedFavouriteItem.currentFavouriteItem = currentItem;
                } 
            }

            if (dragedObject.GetComponent<CurrentItem>())
            {
                if (dragedCurrentItem.currentInventoryItem.id == GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id)
                {
                    if (dragedCurrentItem.currentInventoryItem.isStackable)
                    {
                        int count = dragedCurrentItem.currentInventoryItem.countItem;
                        GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem += count;
                        dragedCurrentItem.currentInventoryItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentFavouriteItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedCurrentItem.currentInventoryItem;
                    dragedCurrentItem.currentInventoryItem = currentFavouriteItem;
                }
            }

            //if (dragedObject.GetComponent<CurrentStorageItem>())
            //{
            //    if (dragedStorageItem.currentStorageItem.id == GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id)
            //    {
            //        if (dragedStorageItem.currentStorageItem.isStackable)
            //        {
            //            int count = dragedStorageItem.currentStorageItem.countItem;
            //            GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem += count;
            //            dragedStorageItem.currentStorageItem = Inventory.instanceInventory.EmptySlot();
            //        }
            //    }
            //    else
            //    {
            //        Item currentFavouriteItem = GetComponent<CurrentFavouriteItem>().currentFavouriteItem;
            //        GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragedStorageItem.currentStorageItem;
            //        dragedStorageItem.currentStorageItem = currentFavouriteItem;
            //    }
            //}
            CardItemManager.instanceInventory.DisplayItems();
            //Storage.DisplayItems();
            CardItemManager.instanceInventory.favourite.DisplayItems();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id == CardItemManager.instanceInventory.EmptySlotID())
            {
                if (dragedCurrentItem)
                {
                    Item dragItem = dragedCurrentItem.currentInventoryItem.getCopy();
                    dragItem.countItem = 1;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragItem;
                    //CardItemManager.instanceInventory.RemoveItem(dragedCurrentItem.ItemNum);
                    CardItemManager.instanceInventory.favourite.DisplayItems();
                    return;
                }
                if (dragedFavouriteItem)
                {
                    Item dragItem = dragedFavouriteItem.currentFavouriteItem.getCopy();
                    dragItem.countItem = 1;
                    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragItem;
                    CardItemManager.instanceInventory.favourite.RemoveItem(dragedFavouriteItem.ItemNum);
                    return;
                }

                //if (dragedStorageItem)
                //{
                //    Item dragItem = dragedStorageItem.currentStorageItem.getCopy();
                //    dragItem.countItem = 1;
                //    GetComponent<CurrentFavouriteItem>().currentFavouriteItem = dragItem;
                //    Storage.RemoveItem(dragedStorageItem.itemNum);
                //    Inventory.instanceInventory.favourite.DisplayItems();
                //    return;
                //}
            }
           
            if (dragedCurrentItem)
            {
                if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id == dragedCurrentItem.currentInventoryItem.id)
                {
                    if (dragedCurrentItem.currentInventoryItem.isStackable)
                    {
                        drag.AddItem(GetComponent<CurrentFavouriteItem>().currentFavouriteItem);
                        //CardItemManager.instanceInventory.RemoveItem(dragedCurrentItem.ItemNum);
                        CardItemManager.instanceInventory.favourite.DisplayItems();
                        return;
                    }
                }
            }

            //if (dragedStorageItem)
            //{
            //    if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id == dragedStorageItem.currentStorageItem.id)
            //    {
            //        if (dragedStorageItem.currentStorageItem.isStackable)
            //        {
            //            drag.AddItem(GetComponent<CurrentFavouriteItem>().currentFavouriteItem);
            //            Storage.RemoveItem(dragedStorageItem.itemNum);
            //            Inventory.instanceInventory.favourite.DisplayItems();
            //            return;
            //        }
            //    }
            //}

            if (dragedFavouriteItem)
            {
                if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.id == dragedFavouriteItem.currentFavouriteItem.id)
                {
                    if (dragedFavouriteItem.currentFavouriteItem.isStackable)
                    {
                        drag.AddItem(GetComponent<CurrentFavouriteItem>().currentFavouriteItem);
                        CardItemManager.instanceInventory.favourite.RemoveItem(dragedFavouriteItem.ItemNum);
                        return;
                    }
                }
            }
        }
        
    }
}
