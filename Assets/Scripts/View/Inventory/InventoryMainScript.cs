using System;
using System.Collections.Generic;
using DatabaseWrapper;
using Extensions;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Главный скрипт инвентаря.
/// </summary>
public class InventoryMainScript : MonoBehaviour
{
    /// <summary>
    /// Список предметов-заглушек (нужен на время тестов)
    /// </summary>
    // private List<Item1> listItems;
    private List<Item> trueListItems;

    /// <summary>
    /// Ссылка на шаблон объекта InventoryItemViewScript
    /// </summary>
    public InventoryItemViewScript InventoryItemView;
    /// <summary>
    /// Ссылка на шаблон объекта InventorySlotScript
    /// </summary>
    public InventorySlotScript InventorySlot;

    /// <summary>
    /// Ссылка на контейнер, отображающий предметы.
    /// </summary>
    private GridLayoutGroup gridLayoutGroup;

    /// <summary>
    /// Ссылка на элемент, динамически создающий слоты.
    /// </summary>
    private InventoryHeap _heap;

    int ITEM_TEMPLATE_ID = 0;

    private void Awake() {
        gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
        _heap = GetComponentInChildren<InventoryHeap>();
    }
    
    void Start()
    {
        // методы инвентаря пока не реализованы
        // var invCont = BaseScripts.StartScript.GetStartScript.InventoryController;
        // var bag = invCont.Bag;

        // поэтому извлекаем предметы напрямую из бд
        var table = DatabaseWrapper.DatabaseWrapper.GetTable($"select id from 'item_template';");
        
        trueListItems = new List<Item>();
        for(int i = 0; i < table.Rows.Count; i++)
        {
            var itemDTO = ItemTemplateRepository.GetById(table.Rows[i].GetInt(ITEM_TEMPLATE_ID));
            trueListItems.Add(new Item(itemDTO));
        }

        // раскомментировать, если не работает бд
        trueListItems.Add(new Item(
            new ItemDto
            {
                IconId = 1,
                Name = "Test Sword",
                FlavorText = "Some text",
                SlotType = InventorySlots.MainHand,
                Quality = ItemQuality.Common
            }
        ));

        trueListItems.Add(new Item(
            new ItemDto
            {
                IconId = 2,
                Name = "Test Sword 1",
                FlavorText = "Some text 1",
                SlotType = InventorySlots.MainHand,
                Quality = ItemQuality.Uncommon
            }
        ));

        var d = new Dictionary<Character.Stats, int>();
        d.Add(Character.Stats.Constitution, 10);
        trueListItems.Add(new Item(
            new ItemDto
            {
                IconId = 3,
                Name = "Test Ring",
                FlavorText = "Some text 3",
                SlotType = InventorySlots.Finger,
                Quality = ItemQuality.Legendary,
                BonusStats = d,
                Flags = ItemFlags.Passive
            }
        ));

        var stackableItem = new Item(
            new ItemDto
            {
                IconId = 4,
                Name = "Test Grass",
                FlavorText = "Some text 4",
                SlotType = InventorySlots.NonEquipable,
                Quality = ItemQuality.Common,
                MaxStack = 5,
                MaxCount = 20,
                Type = ItemClasses.Consumable
            }
        );
        // AddItemCount увеличивает количество только на 1
        stackableItem.AddItemCount();
        stackableItem.AddItemCount();
        stackableItem.AddItemCount();
        stackableItem.AddItemCount();
        trueListItems.Add(stackableItem);
    

        // создаем слоты для предметов
        int k = 0;
        foreach (var item in trueListItems)
        {
            var slot = _heap.GetFreeSlot();
            slot.name = slot.name + k++;
            var iView = Instantiate(InventoryItemView, slot.gameObject.transform);
            iView.name = item.Name;
            iView.SetItem(item);
            slot.AttachNewItemView(iView);
        }
    }

    /// <summary>
    /// Метод фильтрации инвентаря.
    /// </summary>
    /// <param name="filter">Указывает категорию слотов <see cref="InventorySlots"/>,
    /// которые должны быть отображены</param>
    public void FilterInventoryView(InventorySlots filter)
    {
        var allSlots = GetComponentInChildren<ScrollRect>().gameObject.
                             GetComponentsInChildren<InventorySlotScript>(true);

        foreach(var slot in allSlots)
        {
            // выключаем:
            // 1. пустые слоты
            // 2. слоты, которые содержат предмет другого типа
            if(     (slot.ItemView != null) &&
                    ((filter == InventorySlots.NonEquipable) || (slot.ItemView.item.SlotType == filter)))
                slot.gameObject.SetActive(true);
            else
                slot.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Метод выбрасывания предмета из инвентаря на сцену.
    /// </summary>
    /// <param name="iView"></param>
    public void DropItem(InventoryItemViewScript iView)
    {
        var mainPers = GameObject.Find("PaladinGO");
        var pos = new Vector3{
            x = mainPers.transform.position.x,
            y = mainPers.transform.position.y,
            z = mainPers.transform.position.z + 1
        };

        var item = Instantiate(iView.Icon, pos, mainPers.transform.rotation);
        item.gameObject.AddComponent<Canvas>();

        iView.EquipSlot.ItemView = null;
        iView.EquipSlot = null;
        Destroy(iView.gameObject);

        var rect = item.gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(5, 5);
    }

    /// <summary>
    /// Метод для события Drop.
    /// </summary>
    /// <param name="data">Стандартный аргумент события</param>
    public void OnDropItem(BaseEventData data)
    {
        var iView = data.selectedObject.GetComponent<InventoryItemViewScript>();
        DropItem(iView);
    }

    /// <summary>
    /// Метод добавления предмета в инвентарь.
    /// </summary>
    /// <param name="item">Предмет, который нужно добавить</param>
    public void TakeItem(Item item)
    {
        throw new NotImplementedException();
    }
}