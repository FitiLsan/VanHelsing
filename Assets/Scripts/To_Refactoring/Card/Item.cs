using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour {
    public string nameItem;
    [Range(1,999)]
    public int id;
    [HideInInspector]
    public int countItem ;
    public bool isStackable;
    [TextArea(5,100)]
    public string descriptionItem;
    public Sprite Icon;


    public bool isRemovable;
    public bool isDroped;
  
    public UnityEvent OnItemUse;

    public UnityEvent customEvent;

    public Item getCopy()
    {
        return (Item)this.MemberwiseClone();
    }
}
