using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

 
    public static GameObject isDragingObject;

    public static GameObject dragedObject;
    CardItemManager cardItemManager;

  
    void Start()
    {
        cardItemManager = GameObject.FindGameObjectWithTag("Inventory Manager").GetComponent<CardItemManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject dragPrefab = CardItemManager.instanceInventory.dragPrefab;
        isDragingObject = gameObject;
        dragedObject = gameObject;
        cardItemManager.dragPrefab.SetActive(true);
        cardItemManager.dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = false;

        if (dragedObject.GetComponent<CurrentItem>())
        {
            int index = dragedObject.GetComponent<CurrentItem>().index;
            cardItemManager.dragPrefab.GetComponent<Image>().sprite = cardItemManager.item[index].Icon;
            cardItemManager.dragPrefab.transform.GetChild(0);

        }



        if (eventData.button == PointerEventData.InputButton.Left)
        {
            dragPrefab.transform.GetChild(1).GetComponent<Text>().enabled = true;

            if (isDragingObject.GetComponent<CurrentItem>())
            {
                dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentItem>().currentInventoryItem.Icon;
                if (GetComponent<CurrentItem>().currentInventoryItem.countItem > 1)
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().text = GetComponent<CurrentItem>().currentInventoryItem.countItem.ToString();
                else
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().enabled = false;
            }

            if (isDragingObject.GetComponent<CurrentFavouriteItem>())
            {
                dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentFavouriteItem>().currentFavouriteItem.Icon;

                if (GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem > 1)
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().text = GetComponent<CurrentFavouriteItem>().currentFavouriteItem.countItem.ToString();
                else
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().enabled = false;
            }

            if (isDragingObject.GetComponent<CurrentFavouriteoneItem>())
            {
                dragPrefab.GetComponent<Image>().sprite = GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.Icon;

                if (GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.countItem > 1)
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().text = GetComponent<CurrentFavouriteoneItem>().currentFavouriteoneItem.countItem.ToString();
                else
                    dragPrefab.transform.GetChild(1).GetComponent<Text>().enabled = false;
            }



        }
        if (CardItemManager.instanceInventory.dragPrefab.GetComponent<Image>().sprite != null)
        {
            CardItemManager.instanceInventory.dragPrefab.SetActive(false);
            CardItemManager.instanceInventory.dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            isDragingObject = null;
        }
       

        if (dragPrefab.GetComponent<Image>().sprite != null)
        {
            dragPrefab.SetActive(true);
            dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            isDragingObject = null;
        }
    }




    public void OnDrag(PointerEventData eventData)
    {
        CardItemManager.instanceInventory.dragPrefab.transform.position = Input.mousePosition; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragingObject = null;
        CardItemManager.instanceInventory.dragPrefab.SetActive(false);
        GameObject dragPrefab = CardItemManager.instanceInventory.dragPrefab;
        CardItemManager.instanceInventory.dragPrefab.GetComponent<Image>().sprite = null;
       
         CardItemManager.instanceInventory.dragPrefab.GetComponent<CanvasGroup>().blocksRaycasts = true;

        
    }

 

    public void AddItem(Item item)
    {
         item.countItem ++;
    }
}
