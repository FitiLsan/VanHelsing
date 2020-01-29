using UnityEngine;

public class ButtonFilterScript : MonoBehaviour
{
    /// <summary>
    /// Указывае тип отфильтровываемых предметов.
    /// </summary>
    public Items.InventorySlots FilterType;
    public void Filter()
    {
        GetComponentInParent<InventoryMainScript>().FilterInventoryView(FilterType);
    }
}
