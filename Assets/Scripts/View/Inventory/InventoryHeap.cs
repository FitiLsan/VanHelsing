using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryHeap : MonoBehaviour, IDropHandler
{
    public InventorySlotScript InventorySlot;
    private static int k;
    private GridLayoutGroup gridLayoutGroup;

    void Awake()
    {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
    }

    /// <summary>
    /// Предоставляет свободный слот в куче. Если такового нет, то создается новый.
    /// </summary>
    /// <returns></returns>
    public InventorySlotScript GetFreeSlot()
    {
        var slots = GetComponentsInChildren<InventorySlotScript>();

        var freeSlot = slots.FirstOrDefault(s => s.ItemView == null);

        if(!(freeSlot is null))
            return freeSlot;
        else
        {
            return Instantiate(InventorySlot, gridLayoutGroup.transform);
        }
    }

    /// <summary>
    /// Предоставляет все свободные слоты в куче.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<InventorySlotScript> GetAllSlots()
    {
        return GetComponentsInChildren<InventorySlotScript>();
    }

    /// <summary>
    /// Метод для события Drop.
    /// </summary>
    /// <param name="eventData">Стандартный аргумент события</param>
    public void OnDrop(PointerEventData eventData)
    {
        var iView = eventData.selectedObject.GetComponent<InventoryItemViewScript>();
        var slot = GetFreeSlot();
        slot.AttachNewItemView(iView);
    }
}
