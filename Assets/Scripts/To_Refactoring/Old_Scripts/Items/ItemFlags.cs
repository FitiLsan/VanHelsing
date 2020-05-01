using System;

namespace Items
{
    /// <summary>
    /// Дополнительные флаги предметов
    /// </summary>
    [Flags]
    public enum ItemFlags
    {
        ConjuredItem = 0x01, /** Призванный предмет исчезнет при перезаходе в игру*/
        Openable = 0x02, /** можно открыть (сундук) */
        UniqueEquipped = 0x04, /**только один может быть экипирован*/
        Stackable = 0x08, /** собирается в стопку*/
        Passive = 0x10 /**пассивно дает бонусы*/
    }
}