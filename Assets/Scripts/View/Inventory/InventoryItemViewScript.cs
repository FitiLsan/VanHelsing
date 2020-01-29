using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Items;

/// <summary>
/// Скрипт отображения предмета в инвентаре
/// </summary>
public class InventoryItemViewScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Текстовое поле для вывода названия предмета
    /// </summary>
    public Text Name;
    /// <summary>
    /// Такстовое поле для отображения количества "складируемых" предметов
    /// </summary>
    public Text StackCount;
    /// <summary>
    /// Спрайт предмета
    /// </summary>
    public Image Icon;
    /// <summary>
    /// Шаблон tooltip подсказки
    /// </summary>
    public ItemTooltip ToolTipView;
    /// <summary>
    /// Ссылка на экземпляр созданного tooltip
    /// </summary>
    private ItemTooltip _tooltipInstance;

    /// <summary>
    /// Цвет фона, зависящий от редкости придмета
    /// </summary>
    private Image _background;

    /// <summary>
    /// Общее поле для перетакскиваемых предметов (т.к. из-за
    /// порядка иерархии одни предметы могут скрываться под другими)
    /// </summary>
    private GameObject _commonField;

    /// <summary>
    /// Ссылка на предмет, который установлен в текущий слот
    /// </summary>
    [HideInInspector]
    //public Item1 item;
    public Item item;

    /// <summary>
    /// Ссылка на элемент <see cref="CanvasGroup"/> для контроля
    /// свойства RayCast Blocking.
    /// </summary>
    private CanvasGroup _canvasGroup;

    /// <summary>
    /// Ссылка на слот, в который установлен данный ItemView
    /// </summary>
    [HideInInspector]
    public InventorySlotScript EquipSlot;


    private void Awake()
    {
        _background = GetComponent<Image>();
        _commonField = GetComponentInParent<InventoryMainScript>().gameObject;
        _canvasGroup = GetComponent<CanvasGroup>();

        EquipSlot = GetComponentInParent<InventorySlotScript>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent = _commonField.transform;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        // ставим в дефолтное положение
        SetDefaultPosition();
    }

    /// <summary>
    /// Прикрепляет к текущему представлению предмет
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(Item item)
    {
        this.item = item;

        Name.text = item.Name;
        Icon.sprite = Resources.Load<Sprite>("InventorySprites/" + item.SlotType + "/" + item.IconId);
        var tempColor = Icon.color;
        tempColor.a = 255;
        Icon.color = tempColor;

        if(item.MaxStack > 1)
            StackCount.text = $"{item.StackCount}/{item.MaxStack}";
        else
            StackCount.gameObject.SetActive(false);


        _background.color = SetBackgroundColor(item.Quality);
    }

    /// <summary>
    /// Позиционирует ItemView в центре слота
    /// </summary>
    public void SetDefaultPosition()
    {
        transform.parent = EquipSlot.transform;
        transform.localPosition = new Vector3(0,0,0);
    }

    /// <summary>
    /// Метод установки цвета фона ItemView в зависимости от редкости предмета
    /// </summary>
    /// <param name="quality"></param>
    /// <returns></returns>
    private Color SetBackgroundColor(ItemQuality quality)
    {
        Color rarityColor = Color.clear;
        switch (quality)
        {
            case ItemQuality.Poor:
                rarityColor = Color.white;
                break;
            case ItemQuality.Common:
                rarityColor = Color.gray;
                break;
            case ItemQuality.Uncommon:
                rarityColor = Color.green;
                break;
            case ItemQuality.Rare:
                rarityColor = Color.blue;
                break;
            case ItemQuality.Epic:
                rarityColor = Color.yellow;
                break;
            case ItemQuality.Legendary:
                rarityColor = Color.magenta;
                break;
            case ItemQuality.Artifact:
                rarityColor = Color.cyan;
                break;
        }
        return rarityColor;
    }

    /// <summary>
    /// Метод создания Tooltip. Срабатывает при наведении курсора на ItemView.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        var rectT = GetComponent<RectTransform>();
        
        Vector3 pos = new Vector3
        {
            x = rectT.transform.position.x + rectT.rect.width/2 * _commonField.transform.localScale.x,
            y = rectT.transform.position.y - rectT.rect.height/2 * _commonField.transform.localScale.y,
            z = 0
        };

        _tooltipInstance = Instantiate(ToolTipView, pos, new Quaternion(), _commonField.transform);
        _tooltipInstance.SetTooltipInfo(item.Name, item.FlavorText);
    }

    /// <summary>
    /// Метод уничтожения Tooltip. Срабатывает при покидании курсора области ItemView.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(_tooltipInstance.gameObject);
    }
}
