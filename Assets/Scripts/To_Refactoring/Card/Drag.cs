using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    //public int index;
    public static GameObject isDragingObject;



    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject dragPrefab = CardItemManager.instanceInventory.dragPrefab;
        isDragingObject = gameObject;



        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //dragPrefab.transform.GetChild(0).GetComponent<Text>().enabled = true;

            if (isDragingObject.GetComponent<CurrentItem>())
            {
                dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentItem>().currentInventoryItem.Icon;
                //if (GetComponent<CurrentItem>().currentInventoryItem.countItem > 1)
                //    dragPrefab.transform.GetChild(0).GetComponent<Text>().text = GetComponent<CurrentItem>().currentInventoryItem.countItem.ToString();
                //else
                //    dragPrefab.transform.GetChild(0).GetComponent<Text>().enabled = false;
            }

            if (isDragingObject.GetComponent<CurrentFavouriteItem>())
            {
                dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentFavouriteItem>().currentFavouriteItem.Icon;

                //if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem > 1)
                //    dragPrefab.transform.GetChild(0).GetComponent<Text>().text = GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem.ToString();
                //else
                //    dragPrefab.transform.GetChild(0).GetComponent<Text>().enabled = false;
            }

            //if (isDragingObject.GetComponent<CurrentStorageItem>())
            //{
            //    dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentStorageItem>().currentStorageItem.icon;

            //    if (GetComponent<CurrentStorageItem>().currentStorageItem.countItem > 1)
            //        dragPrefab.transform.GetChild(0).GetComponent<Text>().text = GetComponent<CurrentStorageItem>().currentStorageItem.countItem.ToString();
            //    else
            //        dragPrefab.transform.GetChild(0).GetComponent<Text>().enabled = false;
            //}

            if (CardItemManager.instanceInventory.dragPrefab.GetComponent<Image>().sprite != null)
            {
                CardItemManager.instanceInventory.dragPrefab.SetActive(true);
                CardItemManager.instanceInventory.dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                isDragingObject = null;
            }
        }

        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    dragPrefab.transform.GetChild(0).GetComponent<Text>().enabled = false;

        //    if (isDragingObject.GetComponent<CurrentItem>())
        //    {
        //        dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentItem>().currentInventoryItem.Icon;
        //    }

        //    if (isDragingObject.GetComponent<CurrentFavouriteItem>())
        //    {
        //        dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentFavouriteItem>().currentFavouriteItem.Icon;
        //    }

        //    //if (isDragingObject.GetComponent<CurrentStorageItem>())
        //    //{
        //    //    dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentStorageItem>().currentStorageItem.icon;
        //    //}

        //    if (dragPrefab.GetComponent<Image>().sprite != null)
        //    {
        //        dragPrefab.SetActive(true);
        //        dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //    }
        //    else
        //    {
        //        isDragingObject = null;
        //    }
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        CardItemManager.instanceInventory.dragPrefab.transform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            isDragingObject = null;
            GameObject dragPrefab = CardItemManager.instanceInventory.dragPrefab;
        CardItemManager.instanceInventory.dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = true;
        CardItemManager.instanceInventory.dragPrefab.GetComponent<Image>().sprite = null;
        CardItemManager.instanceInventory.dragPrefab.SetActive(false);
    }

    public Item dragedItem(Item item)
    {
        for (int i = 0; i < CardItemManager.instanceInventory.database.transform.childCount; i++)
        {
            Item temp = CardItemManager.instanceInventory.database.transform.GetChild(i).GetComponent<Item>();
            if (temp.id == item.id)
            {
                return temp;
            }
        }
        return null;
    }

    public void AddItem(Item item)
    {
         item.countItem ++;
    }
}
