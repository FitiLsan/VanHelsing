using System;
using System.Collections.Generic;
using System.Linq;
using BaseScripts;
using Character;
using Events;
using Events.Args;
using Interfaces;
using Items;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Контроллер инвентаря и экипировки
    /// </summary>
    public class InventoryController : BaseController
    {
        /// <summary>
        /// Ссылка на объект InventoryMainView в иерархии
        /// </summary>
        private readonly GameObject _inventoryView;

        /// <summary>
        /// Наш инвентарь
        /// </summary>
        private readonly List<Item> _bag;

        /// <summary>
        /// Номер слота правой руки
        /// </summary>
        private readonly int _mainHandSlot;
        /// <summary>
        /// Номер слота левой руки
        /// </summary>
        private readonly int _offHandSlot;

        /// <summary>
        /// Перечисление всех типов двуручного оружия
        /// </summary>
        //TODO: Добавить шенбяо
        private readonly List<ItemClasses> _twoHandedWeapons = new List<ItemClasses>
        {
            ItemClasses.Axe2h,
            ItemClasses.Sword2h,
            ItemClasses.Polearm,
            ItemClasses.FishingPole,
            ItemClasses.Staff,
            ItemClasses.Spear,
            ItemClasses.Bow,
            ItemClasses.Crossbow2h,
            ItemClasses.Gun2h,
            ItemClasses.Mace2h,
            ItemClasses.Shanbao
        };

        /// <summary>
        /// Прослойка для получения предметов и сохранения их
        /// </summary>
        private readonly IItemStorage _storage;

        private readonly InputController _inputController;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="storage">Используемая реализация интерфейса</param>
        public InventoryController(IItemStorage storage)
        {
            _storage = storage;
            _bag = _storage.LoadInventory();
            foreach (var item in _storage.LoadEquipment()) _equipment[item.Key] = item.Value;
            _mainHandSlot = _equipmentSlots.First(x => x.Value == InventorySlots.MainHand).Key;
            _offHandSlot = _equipmentSlots.First(x => x.Value == InventorySlots.OffHand).Key;

            _inputController = StartScript.GetStartScript.InputController;

            _inventoryView = GameObject.Find("InventoryMainView");
            _inventoryView.SetActive(false);
        }

        /// <summary>
        /// Максимально переносимый вес без дополнительных сумок.
        /// </summary>
        public float BaseMaxWeight { get; } = 100f;

        /// <summary>
        /// Текущий вес всех предметов
        /// </summary>
        public float CurrentWeight => Bag.Sum(x => x.StackCount * x.Weight) + GetEquippedWeight();

        /// <summary>
        /// Максимально переносимый вес со всеми сумками
        /// </summary>
        public float MaxWeight => BaseMaxWeight + GetBonusBagsWeight();

        /// <summary>
        /// Доступ к инвентарю только для чтения
        /// </summary>
        public IEnumerable<Item> Bag => _bag.AsReadOnly();

        /// <summary>
        /// Доступ к экипировке только для чтения
        /// </summary>
        public IEnumerable<Item> Equipment => _equipment.AsReadOnly();

        public override void ControllerUpdate()
        {
            //TODO: Receive deltaTime from StartScript
            if (!IsActive) return;
            foreach (var item in _bag)
            {
                item.ReduceDuration(Time.deltaTime);
                if (item.IsExpired) DropItem(item.EntryId);
            }

            // проверка нажатия кнопки инвентаря на клавиатуре
            if(_inputController.Inventory)
                _inventoryView.SetActive(!_inventoryView.activeSelf);
            
            
            // отключение перемещения, пока открыт инвентарь
            if(_inventoryView.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                StartScript.GetStartScript.CameraController.Off();
                StartScript.GetStartScript.MovementController.Off();
            }
            else
            {
                StartScript.GetStartScript.MovementController.On();
                StartScript.GetStartScript.CameraController.On();
            }
        }

        /// <summary>
        /// Подсчет веса экипировки
        /// </summary>
        /// <returns>вес экипировке на персонаже</returns>
        private float GetEquippedWeight()
        {
            return _equipment.Sum(x => x?.Weight ?? 0);
        }

        /// <summary>
        /// Подсчет дополнительного переносимого веса от сумок
        /// </summary>
        /// <returns></returns>
        private float GetBonusBagsWeight()
        {
            var t = _equipmentSlots.Where(x => x.Value == InventorySlots.Bag);
            return t.Sum(pair => _equipment[pair.Key]?.AdditionalWeight ?? 0);
        }
        
        /// <summary>
        /// Подбираем вещь
        /// </summary>
        /// <param name="item">Вещь</param>
        /// <returns>Смогли взять или нет</returns>
        public bool TakeItem(Item item)
        {
            if (CurrentWeight + item.Weight >= MaxWeight) return false;
            var tmp = _bag.FindAll(x => x.Id == item.Id);
            foreach (var item1 in tmp)
            {
                if (item1.IsMaxCountAlready) return false;
                if ((item1.Flags & ItemFlags.Stackable) <= 0) break;
                if (item1.IsMaxStack) continue;
                item1.AddItemCount();
                return true;
            }

            _bag.Add(item);
            //TODO: Start Script if needed
            //TODO: Save in save-file and provide entryId
            return true;
        }

        /// <summary>
        /// Выкидываем вещь
        /// </summary>
        /// <param name="entry">Ид пользовательской копии предмета</param>
        /// <param name="triggerEvent">Запускаем ли событие</param>
        /// <returns>Смогли выкинуть или нет</returns>
        public bool DropItem(int entry, bool triggerEvent = true)
        {
            var tmp = _bag.Find(x => x.EntryId == entry);
            if (tmp == null) return false;
            _bag.Remove(tmp);
            if (triggerEvent)
                EventManager.TriggerEvent(GameEventTypes.ItemDrop, new ItemArgs(tmp));
            return true;
        }

        /// <summary>
        /// Считаем бонусы от предметов в экипировке
        /// </summary>
        /// <returns>Словарь стат-значение бонуса</returns>
        public Dictionary<Stats, int> GetBonusStats()
        {
            var res = new Dictionary<Stats, int>();

            foreach (var item in _equipment)
            {
                if (item == null) continue;
                if ((item.Flags & ItemFlags.Passive) <= 0) continue;
                foreach (var bonusStat in item.BonusStats)
                    if (res.ContainsKey(bonusStat.Key))
                        res[bonusStat.Key] += bonusStat.Value;
                    else
                        res.Add(bonusStat.Key, bonusStat.Value);
            }

            return res;
        }

        /// <summary>
        /// Считаем пассивные бонусы в инвентаре
        /// </summary>
        /// <returns>Словарь стат-значение бонуса</returns>
        public Dictionary<Stats, int> GetPassiveBonuses()
        {
            var res = new Dictionary<Stats, int>();

            foreach (var item in Bag)
            {
                if (item == null) continue;
                if ((item.Flags & ItemFlags.Passive) <= 0) continue;
                foreach (var bonusStat in item.BonusStats)
                    if (res.ContainsKey(bonusStat.Key))
                        res[bonusStat.Key] += bonusStat.Value;
                    else
                        res.Add(bonusStat.Key, bonusStat.Value);
            }

            return res;
        }

        /// <summary>
        /// Надеваем предмет
        /// </summary>
        /// <param name="item">предмет</param>
        /// <returns>Смогли надеть или нет</returns>
        public bool PutOn(Item item)
        {
            var slot = _equipmentSlots.FirstOrDefault(x => x.Value == item.SlotType);
            if (slot.Equals(default(KeyValuePair<int, InventorySlots>))) return false;

            //TODO: Check for restrictions (stats, level) (when character model will be ready)

            if (_twoHandedWeapons.Contains(item.Type))
            {
                TakeOff(_mainHandSlot);
                TakeOff(_offHandSlot);
            }
            else if (item.SlotType == InventorySlots.OffHand &&
                     _twoHandedWeapons.Contains(_equipment[_mainHandSlot].Type))
            {
                TakeOff(_mainHandSlot);
            }
            else
            {
                TakeOff(slot.Key);
            }

            _equipment[slot.Key] = item;
            EventManager.TriggerEvent(GameEventTypes.EquipmentChanged, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Снимаем предмет
        /// </summary>
        /// <param name="slot">Из какого слота снимаем</param>
        /// <returns>Смогли снять или нет</returns>
        public bool TakeOff(int slot)
        {
            if (!_equipmentSlots.ContainsKey(slot) || _equipment[slot] == null) return false;
            TakeItem(_equipment[slot]);
            _equipment[slot] = null;
            EventManager.TriggerEvent(GameEventTypes.EquipmentChanged, EventArgs.Empty);
            return true;
        }

        #region EquipmentSlots

        /// <summary>
        /// Экипировка
        /// </summary>
        private readonly List<Item> _equipment = new List<Item>
        {
            null, //Head
            null, //Neck
            null, //Shoulder
            null, //Chest,
            null, //Waist
            null, //Legs
            null, //Feet
            null, //Wrist
            null, //Hand
            null, //Finger1
            null, //Finger2
            null, //Trinket1
            null, //Trinket2
            null, //Back
            null, //MainHand or two-handed
            null, //OffHand
            null, //Ammo
            null, //Bag1
            null, //Bag2
            null //Bag3
        };

        //Definitions of slots

        /// <summary>
        /// Номера слотов в соответствии к типу предмета для этого слота
        /// </summary>
        private readonly Dictionary<int, InventorySlots> _equipmentSlots = new Dictionary<int, InventorySlots>
        {
            {0, InventorySlots.Head},
            {1, InventorySlots.Neck},
            {2, InventorySlots.Shoulder},
            {3, InventorySlots.Chest},
            {4, InventorySlots.Waist},
            {5, InventorySlots.Legs},
            {6, InventorySlots.Feet},
            {7, InventorySlots.Wrist},
            {8, InventorySlots.Hands},
            {9, InventorySlots.Finger},
            {10, InventorySlots.Finger},
            {11, InventorySlots.Trinket},
            {12, InventorySlots.Trinket},
            {13, InventorySlots.Back},
            {14, InventorySlots.MainHand},
            {15, InventorySlots.OffHand},
            {16, InventorySlots.Ammo},

            {17, InventorySlots.Bag},
            {18, InventorySlots.Bag},
            {19, InventorySlots.Bag}
        };

        #endregion
    }
}