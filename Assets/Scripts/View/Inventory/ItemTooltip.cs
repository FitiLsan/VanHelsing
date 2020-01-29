using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    /// <summary>
    /// Текстовое поле названия предмета.
    /// </summary>
    public Text Name;

    /// <summary>
    /// Текстовое поле описания предмета.
    /// </summary>
    public Text Description;

    /// <summary>
    /// Метод установки значений полей tooltip.
    /// </summary>
    /// <param name="name">Название предмета</param>
    /// <param name="desc">Описание предмета</param>
    public void SetTooltipInfo(string name, string desc)
    {
        Name.text = name;
        Description.text = desc;
    }
}
