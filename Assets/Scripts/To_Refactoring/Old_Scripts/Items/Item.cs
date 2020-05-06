using System.Collections.Generic;
using Character;
using Common;

namespace Items
{
    /// <summary>
    /// Класс предметов
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="dto">шаблон</param>
        public Item(ItemDto dto)
        {
            Id = dto.Id;
            Type = dto.Type;
            ModelId = dto.ModelId;
            IconId = dto.IconId;
            Flags = dto.Flags;
            Quality = dto.Quality;
            BuyPrice = dto.BuyPrice;
            SellPrice = dto.SellPrice;
            SlotType = dto.SlotType;
            Level = dto.Level;
            RequiredLevel = dto.RequiredLevel;
            MaxCount = dto.MaxCount;
            Weight = dto.Weight;
            AdditionalWeight = dto.AdditionalWeight;
            RequiredStats = dto.RequiredStats;
            BonusStats = dto.BonusStats;
            DmgMin1 = dto.DmgMin1;
            DmgMax1 = dto.DmgMax1;
            DmgType1 = dto.DmgType1;
            DmgMin2 = dto.DmgMin2;
            DmgMax2 = dto.DmgMax2;
            DmgType2 = dto.DmgType2;
            Armor = dto.Armor;
            BaseDelay = dto.BaseDelay;
            AmmoType = dto.AmmoType;
            Range = dto.Range;
            PageId = dto.PageId;
            StartQuest = dto.StartQuest;
            Sheath = dto.Sheath;
            LockId = dto.LockId;
            BaseBlockChance = dto.BaseBlockChance;
            BaseBlockValue = dto.BaseBlockValue;
            SetId = dto.SetId;
            MaxDurability = dto.MaxDurability;
            UsableInZones = dto.UsableInZones;
            ScriptName = dto.ScriptName;
            MaxDuration = dto.MaxDuration;
            Name = dto.Name;
            FlavorText = dto.FlavorText;

            CurrentDuration = MaxDuration;
            CurrentDurability = MaxDurability;
            ScriptUsed = false;
            MaxStack = dto.MaxStack;
            StackCount = 1;
        }

        /// <summary>
        /// Ид шаблона
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Ид копии у игрока
        /// </summary>
        public int EntryId { get; private set; }
        /// <summary>
        /// Тип предмета
        /// </summary>
        public ItemClasses Type { get; }
        /// <summary>
        /// Ид модели для отображения
        /// </summary>
        public int ModelId { get; }
        /// <summary>
        /// Ид иконки для отображения
        /// </summary>
        public int IconId { get; }
        /// <summary>
        /// Дополнительные флаги предмета
        /// </summary>
        public ItemFlags Flags { get; }
        /// <summary>
        /// Качество предмета
        /// </summary>
        public ItemQuality Quality { get; }
        /// <summary>
        /// Цена покупки у торговца
        /// </summary>
        public int BuyPrice { get; }
        /// <summary>
        /// Цена продажи торговцу
        /// </summary>
        public int SellPrice { get; }
        /// <summary>
        /// Слот экипировки
        /// </summary>
        public InventorySlots SlotType { get; }
        /// <summary>
        /// Уровень предмета
        /// </summary>
        public int Level { get; }
        /// <summary>
        /// Минимальный уровень игрока для использования предмета
        /// </summary>
        public int RequiredLevel { get; }
        /// <summary>
        /// Максимум у игрока
        /// </summary>
        public int MaxCount { get; }
        /// <summary>
        /// Вес предмета
        /// </summary>
        public float Weight { get; }
        /// <summary>
        /// Если это сумка, то какой бонус к переносимому весу она дает
        /// </summary>
        public float AdditionalWeight { get; }
        /// <summary>
        /// Необходимые для ношения характеристики
        /// </summary>
        public Dictionary<Stats, int> RequiredStats { get; } = new Dictionary<Stats, int>();
        /// <summary>
        /// Бонусные характеристики
        /// </summary>
        public Dictionary<Stats, int> BonusStats { get; } = new Dictionary<Stats, int>();
        /// <summary>
        /// Минимальный урон первой атаки
        /// </summary>
        public int DmgMin1 { get; }
        /// <summary>
        /// Максимальный урон первой атаки
        /// </summary>
        public int DmgMax1 { get; }
        /// <summary>
        /// Тип урона первой атаки
        /// </summary>
        public DamageTypes DmgType1 { get; }
        /// <summary>
        /// Минимальный урон второй атаки
        /// </summary>
        public int DmgMin2 { get; }
        /// <summary>
        /// Максимальный урон второй атаки
        /// </summary>
        public int DmgMax2 { get; }
        /// <summary>
        /// Тип урона второй атаки
        /// </summary>
        public DamageTypes DmgType2 { get; }
        /// <summary>
        /// Бонус к армору
        /// </summary>
        public int Armor { get; }
        /// <summary>
        /// Базовая задержка между ударами в мс
        /// </summary>
        public int BaseDelay { get; }
        /// <summary>
        /// Какие патроны использует
        /// </summary>
        public ItemClasses AmmoType { get; }

        /// <summary>
        /// Дальность атаки или использования
        /// </summary>
        public float Range { get; }

        //TODO: Spells & Effects
        /// <summary>
        /// Если книга, то какая страница должна отображаться при чтении
        /// </summary>
        public int PageId { get; }
        /// <summary>
        /// Ид квеста, который начинается этим предметом
        /// </summary>
        public int StartQuest { get; }
        /// <summary>
        /// Как убирается оружие
        /// </summary>
        public SheathTypes Sheath { get; }
        /// <summary>
        /// Ид ключа для этого предмета
        /// </summary>
        public int LockId { get; }
        /// <summary>
        /// Базовый шангс блока щитом
        /// </summary>
        public float BaseBlockChance { get; }
        /// <summary>
        /// Базовая величина блока
        /// </summary>
        public int BaseBlockValue { get; }
        /// <summary>
        /// Ид сета предметов
        /// </summary>
        public int SetId { get; }
        /// <summary>
        /// Максимальная прочность
        /// </summary>
        public int MaxDurability { get; }
        /// <summary>
        /// Текущая прочность
        /// </summary>
        public int CurrentDurability { get; private set; }
        /// <summary>
        /// В каких зонах можно использовать
        /// </summary>
        public List<int> UsableInZones { get; } = new List<int>();
        /// <summary>
        /// Имя доп скрипта
        /// </summary>
        public string ScriptName { get; }
        /// <summary>
        /// Был ли этот скрипт уже активирован
        /// </summary>
        public bool ScriptUsed { get; set; }
        /// <summary>
        /// Максимальное время существования предмета сек
        /// </summary>
        public float MaxDuration { get; }
        /// <summary>
        /// Сколько предмет еще будет существовать сек.
        /// </summary>
        public float CurrentDuration { get; private set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Дополнительное описание
        /// </summary>
        public string FlavorText { get; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Текущее количество в пачке
        /// </summary>
        public int StackCount { get; private set; }
        /// <summary>
        /// Максимальное количество в одной пачке
        /// </summary>
        public int MaxStack { get; }

        /// <summary>
        /// Сломан ли предмет
        /// </summary>
        public bool IsItemBroken => CurrentDurability == 0;

        /// <summary>
        /// Кончилось ли у предмета время существования
        /// </summary>
        public bool IsExpired => MaxDuration > 0 && CurrentDuration <= 0;

        /// <summary>
        /// Не достигли ли мы максимума таких предметов
        /// </summary>
        public bool IsMaxCountAlready => Count >= MaxCount;

        /// <summary>
        /// Не достигли ли мы максимума одной пачки
        /// </summary>
        public bool IsMaxStack => StackCount >= MaxStack;
        /// <summary>
        /// Уменьшаем прочность
        /// </summary>
        /// <param name="value">на сколько уменьшаем</param>

        public void ReduceDurability(int value)
        {
            CurrentDurability -= value;
            if (CurrentDurability < 0) CurrentDurability = 0;
        }

        /// <summary>
        /// Чиним предмет
        /// </summary>
        public void Repair()
        {
            CurrentDurability = MaxDurability;
        }

        /// <summary>
        /// Уменьшаем оставшееся время существоания
        /// </summary>
        /// <param name="delta">на сколько уменьтшаем</param>
        public void ReduceDuration(float delta)
        {
            if (MaxDuration > 0)
                CurrentDuration -= delta;
        }

        /// <summary>
        /// Устанавливаем Ид копии
        /// </summary>
        /// <param name="id">ид</param>
        public void SetEntryId(int id)
        {
            EntryId = id;
        }

        /// <summary>
        /// Добавляем предмет
        /// </summary>
        /// <param name="value">сколько предметов</param>
        /// <returns>Смогли ли добавить</returns>
        public bool AddItemCount(int value = 1)
        {
            if (IsMaxCountAlready) return false;
            if (IsMaxStack) return false;
            Count++;
            StackCount++;
            return true;
        }
    }
}