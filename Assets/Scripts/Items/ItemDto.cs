using System.Collections.Generic;
using Character;
using Common;

namespace Items
{
    /// <summary>
    /// Шаблон предмета. этот класс используется для передачи данных
    /// </summary>
    public class ItemDto
    {
        public int Id { get; set; }
        public ItemClasses Type { get; set; }
        public int ModelId { get; set; }
        public int IconId { get; set; }
        public ItemFlags Flags { get; set; }
        public ItemQuality Quality { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public InventorySlots SlotType { get; set; }
        public int Level { get; set; }
        public int RequiredLevel { get; set; }
        public int MaxCount { get; set; }
        public float Weight { get; set; }
        public float AdditionalWeight { get; set; }
        public Dictionary<Stats, int> RequiredStats { get; set; } = new Dictionary<Stats, int>();
        public Dictionary<Stats, int> BonusStats { get; set; } = new Dictionary<Stats, int>();
        public int DmgMin1 { get; set; }
        public int DmgMax1 { get; set; }
        public DamageTypes DmgType1 { get; set; }
        public int DmgMin2 { get; set; }
        public int DmgMax2 { get; set; }
        public DamageTypes DmgType2 { get; set; }
        public int Armor { get; set; }
        public int BaseDelay { get; set; }
        public ItemClasses AmmoType { get; set; }

        public float Range { get; set; }

        //TODO: Spells & Effects
        public int PageId { get; set; }
        public int StartQuest { get; set; }
        public SheathTypes Sheath { get; set; }
        public int LockId { get; set; }
        public float BaseBlockChance { get; set; }
        public int BaseBlockValue { get; set; }
        public int SetId { get; set; }
        public int MaxDurability { get; set; }
        public List<int> UsableInZones { get; set; } = new List<int>();
        public string ScriptName { get; set; }
        public float MaxDuration { get; set; }

        public string Name { get; set; }
        public string FlavorText { get; set; }

        public int MaxStack { get; set; }
    }
}