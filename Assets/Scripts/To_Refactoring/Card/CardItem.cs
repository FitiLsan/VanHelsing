using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CardItem : MonoBehaviour
{
    public string NameItem;
    public int Id;

    [TextArea(5, 100)]
    public string descriptionBadItem;
    [TextArea(5, 100)]
    public string DescriptionGoodItem;
    public string PathIcon;
    public string pathPrefab;
    public Sprite Sprite;


   public CardItem() 
    {
    
    
    }
    public CardItem(int id, string nameItem, string descriptionGoodItem, Sprite sprite) 
    {
        Id = id;
        NameItem = nameItem;
        DescriptionGoodItem = descriptionGoodItem;
        Sprite = sprite;
        
    }

}




