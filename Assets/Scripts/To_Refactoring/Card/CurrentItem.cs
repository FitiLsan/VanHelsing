using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class CurrentItem : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    private int itemNum;
    public int index;
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

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentInventoryItem.OnItemUse != null)
            {
                currentInventoryItem.OnItemUse.Invoke();
            }
            if (currentInventoryItem.isRemovable)
            {
                CardItemManager.instanceInventory.RemoveItem(ItemNum);
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (currentInventoryItem.isDroped)
            {
                CardItemManager.instanceInventory.DropedItem(currentInventoryItem.id);
                CardItemManager.instanceInventory.RemoveItem(ItemNum);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = Drag.isDragingObject;

        if (dragedObject == null)
        {
            return;
        }

        CurrentItem dragedCurrentItem = dragedObject.GetComponent<CurrentItem>();
        CurrentFavouriteItem dragedFavouriteItem = dragedObject.GetComponent<CurrentFavouriteItem>();
        //CurrentStorageItem dragedStorageItem = dragedObject.GetComponent<CurrentStorageItem>();

        Drag drag = Drag.isDragingObject.GetComponent<Drag>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (dragedObject.GetComponent<CurrentItem>())
            {
                if (dragedCurrentItem.currentInventoryItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
                {
                    if (dragedCurrentItem.currentInventoryItem.isStackable)
                    {
                        int count = dragedCurrentItem.currentInventoryItem.countItem;
                        GetComponent<CurrentItem>().currentInventoryItem.countItem += count;
                        dragedCurrentItem.currentInventoryItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
                    GetComponent<CurrentItem>().currentInventoryItem = dragedCurrentItem.currentInventoryItem;
                    dragedCurrentItem.currentInventoryItem = currentItem;
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
                        dragedFavouriteItem.currentFavouriteItem = CardItemManager.instanceInventory.EmptySlot();
                    }
                }
                else
                {
                    Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
                    GetComponent<CurrentItem>().currentInventoryItem = dragedFavouriteItem.currentFavouriteItem;
                    dragedFavouriteItem.currentFavouriteItem = currentItem;
                }
            }

            //if (dragedObject.GetComponent<CurrentStorageItem>())
            //{
            //    if (dragedStorageItem.currentStorageItem.id == GetComponent<CurrentItem>().currentInventoryItem.id)
            //    {
            //        if (dragedStorageItem.currentStorageItem.isStackable)
            //        {
            //            int count = dragedStorageItem.currentStorageItem.countItem;
            //            GetComponent<CurrentItem>().currentInventoryItem.countItem += count;
            //            dragedStorageItem.currentStorageItem = Inventory.instanceInventory.EmptySlot();
            //        }
            //    }
            //    else
            //    {
            //        Item currentItem = GetComponent<CurrentItem>().currentInventoryItem;
            //        GetComponent<CurrentItem>().currentInventoryItem = dragedStorageItem.currentStorageItem;
            //        dragedStorageItem.currentStorageItem = currentItem;
            //    }
            //}

            CardItemManager.instanceInventory.DisplayItems();
            //Storage.DisplayItems();
            if (CardItemManager.instanceInventory.favourite)
                CardItemManager.instanceInventory.favourite.DisplayItems();
        }

        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    if (GetComponent<CurrentItem>().currentInventoryItem.id == CardItemManager.instanceInventory.EmptySlotID())
        //    {
        //        if (dragedCurrentItem)
        //        {
        //            Item dragItem = dragedCurrentItem.currentInventoryItem.getCopy();
        //            dragItem.countItem = 1;
        //            GetComponent<CurrentItem>().currentInventoryItem = dragItem;
        //            CardItemManager.instanceInventory.RemoveItem(dragedCurrentItem.ItemNum);
        //            return;
        //        }

        //        if (dragedFavouriteItem)
        //        {
        //            Item dragItem = dragedFavouriteItem.currentFavouriteItem.getCopy();
        //            dragItem.countItem = 1;
        //            GetComponent<CurrentItem>().currentInventoryItem = dragItem;
        //            CardItemManager.instanceInventory.favourite.RemoveItem(dragedFavouriteItem.ItemNum);
        //            CardItemManager.instanceInventory.DisplayItems();
        //            return;
        //        }
        //        //if (dragedStorageItem)
        //        //{
        //        //    Item dragItem = dragedStorageItem.currentStorageItem.getCopy();
        //        //    dragItem.countItem = 1;
        //        //    GetComponent<CurrentItem>().currentInventoryItem = dragItem;
        //        //    Storage.RemoveItem(dragedStorageItem.itemNum);
        //        //    Inventory.instanceInventory.DisplayItems();
        //        //    return;
        //        //}
        //    }

        //    if (dragedCurrentItem)
        //    {
        //        if (GetComponent<CurrentItem>().currentInventoryItem.id == dragedCurrentItem.currentInventoryItem.id)
        //        {
        //            if (dragedCurrentItem.currentInventoryItem.isStackable)
        //            {
        //                drag.AddItem(GetComponent<CurrentItem>().currentInventoryItem);
        //                CardItemManager.instanceInventory.RemoveItem(dragedCurrentItem.ItemNum);
        //                return;
        //            }
        //        }
        //    }
    
            if (dragedFavouriteItem)
            {
                if (GetComponent<CurrentItem>().currentInventoryItem.id == dragedFavouriteItem.currentFavouriteItem.id)
                {
                    if (dragedFavouriteItem.currentFavouriteItem.isStackable)
                    {
                        drag.AddItem(GetComponent<CurrentItem>().currentInventoryItem);
                        CardItemManager.instanceInventory.favourite.RemoveItem(dragedFavouriteItem.ItemNum);
                        CardItemManager.instanceInventory.DisplayItems();
                        return;
                    }
                }
            }

            //if (dragedStorageItem)
            //{
            //    if (GetComponent<CurrentItem>().currentInventoryItem.id == dragedStorageItem.currentStorageItem.id)
            //    {
            //        if (dragedStorageItem.currentStorageItem.isStackable)
            //        {
            //            drag.AddItem(GetComponent<CurrentItem>().currentInventoryItem);
            //            Storage.RemoveItem(dragedStorageItem.itemNum);
            //            Inventory.instanceInventory.DisplayItems();
            //            return;
            //        }
            //    }
            //}
        }
    }










