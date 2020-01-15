using UnityEngine;
using Items;
using System.Linq;
using UnityEngine.EventSystems;

/// <summary>
/// Скрипт слота экипировки. Инкапсулирует логику размещения предмета в слоте, 
/// в зависимости от типа слота.
/// </summary>
public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    /// <summary>
    /// Определяет, предметы какого класса
    /// могут быть установлены в нем. Если тип слота <see cref="Items.InventorySlots.NonEquipable"/>,
    /// то принимает любой предмет.
    /// </summary>
    public  InventorySlots SlotClass;
    
    /// <summary>
    /// Ссылка на прикрепленное ItemView
    /// </summary>
    [HideInInspector]
    public InventoryItemViewScript ItemView;

    public void AttachNewItemView(InventoryItemViewScript newItemView)
    {
        // если перетащили на слот экипировки
        if(SlotClass == newItemView.item.SlotType)
        {
            // если в слоте уже установлен предмет
            if(!(ItemView is null))
                SwapItemView(newItemView);
            
            else
            {
                newItemView.EquipSlot.ItemView = null;
                SetItemViewIntoSlot(newItemView, this);
            }
            
            BaseScripts.StartScript.GetStartScript.InventoryController.PutOn(newItemView.item);
        }
        // если перетащили на слот общего назначения
        else if(SlotClass == InventorySlots.NonEquipable)
        {
            // и ItemView уже был установлен в слоте NonEquipable
            if(newItemView.EquipSlot.SlotClass == InventorySlots.NonEquipable)
            {
                // меняем их местами
                if(!(ItemView is null))  
                    SwapItemView(newItemView);
                
                else
                {
                    newItemView.EquipSlot.ItemView = null;
                    SetItemViewIntoSlot(newItemView, this);
                }
            }
            // и ItemView уже был установлен в слоте экипировки
            else
            {
                // если слот пустой, то просто установим в него
                if(ItemView is null)
                {
                    newItemView.EquipSlot.ItemView = null;
                    SetItemViewIntoSlot(newItemView, this);
                }
                // иначе просим выдать пустой слот
                else
                {
                    var heap = ItemView.GetComponentInParent<InventoryHeap>();
                    var freeSlot = heap.GetFreeSlot();

                    if(!(freeSlot is null))
                    {
                        newItemView.EquipSlot.ItemView = null;
                        newItemView.EquipSlot = freeSlot;
                        freeSlot.ItemView = newItemView;
                    }

                    newItemView.SetDefaultPosition();
                }

                var equipment = BaseScripts.StartScript.GetStartScript.InventoryController.Equipment;
                var slotNum = equipment.ToList().IndexOf(newItemView.item);
                BaseScripts.StartScript.GetStartScript.InventoryController.TakeOff(slotNum);
            }
        }
        else
            newItemView.SetDefaultPosition();
    }

    /// <summary>
    /// Метод для события Drop.
    /// </summary>
    /// <param name="eventData">Стандартный аргумент события</param>
    public void OnDrop(PointerEventData eventData)
    {
        var iView = eventData.selectedObject.GetComponent<InventoryItemViewScript>();
        AttachNewItemView(iView);
    }

    /// <summary>
    /// Метод перестановки местами текущей и новой ItemView
    /// </summary>
    /// <param name="newItemView">Новаое ItemView</param>
    private void SwapItemView(InventoryItemViewScript newItemView)
    {
        var newEqSlot = newItemView.EquipSlot;
        var oldItemView = ItemView;

        SetItemViewIntoSlot(newItemView, this);
        SetItemViewIntoSlot(oldItemView, newEqSlot);
    }

    /// <summary>
    /// Метод установки ItemView в Slot 
    /// </summary>
    /// <param name="iView"></param>
    /// <param name="slot"></param>
    private void SetItemViewIntoSlot(InventoryItemViewScript iView, InventorySlotScript slot)
    {
        iView.EquipSlot = slot;
        slot.ItemView = iView;
        iView.SetDefaultPosition();
    }
}
