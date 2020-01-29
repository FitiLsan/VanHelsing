using System;
using System.Collections.Generic;
using Character;
using Models;
using UnityEngine;
using BaseScripts;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(SphereCollider))]
    public class CharController : MonoBehaviour
    {
        private Rigidbody _charBody;
        [HideInInspector] public Vector3 charCenter; // Центр перса
        private CapsuleCollider _charCollider;
        /// <summary>
        /// Хранить состояние персонажа (базовые и текущие характеристики)
        /// </summary>
        private BaseCharacterModel_ver2 _charCharacteristic;
        /// <summary>
        /// Ссылка на объект, содержащий значения базовых статов и 
        /// персонажа параметров. После переноса значений статов в 
        /// _charCharacteristic, ссылается на null.
        /// </summary>
        [SerializeField]
        private CharacterBaseStats _baseStats;
        /// <summary>
        /// Хранит физичекие параметры модели
        /// </summary>
        [SerializeField]
        private PhysicsParams _physicsParams;
        private InventoryController _inventory;

        [HideInInspector] public bool charIsGrounded;

        #region Физические параметры

        public float JumpForce { get => _physicsParams.JumpForce; } // Сила прыжка
        public float RollSpeed { get => _physicsParams.RollSpeed; } // Сила кувырка
        public float FallForce { get => _physicsParams.FallForce; } // Скорость падения
        public float Height { get => _physicsParams.Height; } // Высота перса
        public float Mass { get => _physicsParams.Mass; } // Масса игрока
        public float Drag { get => _physicsParams.Drag; } //Сопротивление игрока
        public float Radius { get => _physicsParams.Radius; } // Радиус
        public float Speed { get => _physicsParams.Speed; } // Скорость перса

        #endregion


        #region Свойства для быстрого доступа к нек. характеристикам из вне

        public int HealthPoints { get => _charCharacteristic.HealthPoints; }
        public int ManaPoints { get => _charCharacteristic.ManaPoints; }
        public int StaminaPoints { get => _charCharacteristic.StaminaPoints; }
        public int MoralPoints { get => _charCharacteristic.MoralPoints; }
        public int MaxHealthPoints { get => _charCharacteristic.MaxHealthPoints; }
        public int MaxManaPoints { get => _charCharacteristic.MaxManaPoints; }
        public int MaxStaminaPoints { get => _charCharacteristic.MaxStaminaPoints; }
        public int MaxMoralPoints { get => _charCharacteristic.MaxMoralPoints; }
        
        #endregion

        private SphereCollider _foot;

        public Rigidbody Rigidbody { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }

        private void Start()
        {
            Cursor.visible = false;
           // Cursor.lockState = CursorLockMode.Locked;
            _charBody = GetComponent<Rigidbody>();
            Rigidbody = _charBody;
            _charBody.constraints =
                RigidbodyConstraints.FreezeRotation; // Заморозка вращения что бы не падал по сторонам
            _charBody.mass = _physicsParams.Mass;

            _charCollider = GetComponent<CapsuleCollider>(); // Ссылка на коллайдер
            CapsuleCollider = _charCollider;
            charCenter = new Vector3(-0.15f, -0.65f, 0f); // Задаем центр (найден имперически)
            // Параметры коллайдера
            _charCollider.center = charCenter;
            _charCollider.height = _physicsParams.Height;
            _charCollider.radius = _physicsParams.Radius;
            _charBody.drag = _physicsParams.Drag;
            // Коллайдер для проверки земли
            _foot = GetComponent<SphereCollider>();
            _foot.center = new Vector3(-0.15f, -_charCollider.height * 0.75f, 0f);
            _foot.radius = _charCollider.radius;
            _foot.isTrigger = true;

            _inventory = BaseScripts.StartScript.GetStartScript.InventoryController;

            // заполняем модель состояния персонажа
            _charCharacteristic = new BaseCharacterModel_ver2(_baseStats);
            _baseStats = null; // чтоб не хранить в памяти лишнего

            Events.EventManager.StartListening(Events.GameEventTypes.EquipmentChanged, UpdateStats);

            // сразу обновим статы
            // null - т.к. никакие параметры не нужны, а этот же метод используется в 
            // системе событий
            UpdateStats(null);
            
            // примеры использования
            // int a = GetStatValue(Stats.AlchemicalResistance);
            // var constitutionList = GetAllStatsInGroup(Stats.Constitution);
        }

        /// <summary>
        ///     Метод передвижения
        /// </summary>
        /// <param name="movement">Вектор движения</param>
        public void Move(Vector3 movement)
        {
            Rigidbody.MovePosition(transform.position + movement);
        }

        public void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            charIsGrounded = true;
        }

        public void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            charIsGrounded = false;
        }

                /// <summary>
        /// Возвращает значение конкретной характеристики.
        /// </summary>
        /// <param name="specificStat"></param>
        /// <returns></returns>
        public int GetStatValue(Stats specificStat)
        {
            return _charCharacteristic.StatsList[specificStat];
        }

        
        /// <summary>
        /// Взвращает перечисление, содержащее все статы указанной группы.
        /// </summary>
        /// <param name="group">Название группы</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<Stats, int>> GetAllStatsInGroup(Stats group)
        {
            if(((int)group & 0x0f) != 0x01) // проверка: не указана ли по ошибке конкретная стата
                throw new ArgumentException($"Х-ка {group} не является базовой.");

            List<KeyValuePair<Stats, int>> tempList = new List<KeyValuePair<Stats, int>>();

            foreach(var pair in _charCharacteristic.StatsList)
            {
                int currentStatCode = (int)pair.Key;

                // проверка на принадлежность группе
                if((currentStatCode & 0xf0) == ((int)group & 0xf0))
                    tempList.Add(pair);
            }

            return tempList;
        }

        /// <summary>
        /// Метод для события <see cref="Events.GameEventTypes.EquipmentChanged"/>
        /// </summary>
        /// <param name="args"></param>
        private void UpdateStats(EventArgs args)
        {
            // завел делегат, т.к. длинное название получилось
            Func<StatContainer, Stats, int> calcBase = BaseStatsDependency.ParentToChildCalculation;
            
            var bonusStats = _inventory.GetBonusStats();
            var passiveBonus = _inventory.GetPassiveBonuses();
            var playerAssigment = PlayerStatsAssigment.GetStatsAssigment();
            /* здесь могут быть ваши  бафы */

            StatContainer parent = null;
            foreach (var stat in (Stats[])Enum.GetValues(typeof(Stats)))
            {
                int totalStatValue = 0;

                #region Расчет значений, даваемых предметами
                bonusStats.TryGetValue(stat, out int bonusValue);
                passiveBonus.TryGetValue(stat, out int passiveValue);
                totalStatValue = bonusValue +  passiveValue;
                #endregion

                #region Добавление основных значения
                if (((int)stat & 0x0f) == 0x01)
                {
                    totalStatValue = totalStatValue + 
                                        _charCharacteristic.BaseStatsList[stat] + 
                                            playerAssigment[stat];
                    // сохраняем ссылку на родительскую стату, чтоб использовать ее при
                    // расчете дочерней
                    parent = new StatContainer(stat, totalStatValue);
                }
                else
                    // дочерняя включает:
                    //  1. расчет от базовой
                    //  2. дополнительно назначенное игроком
                    totalStatValue = totalStatValue + calcBase(parent, stat) + playerAssigment[stat];
                #endregion

                _charCharacteristic.StatsList[stat] = totalStatValue;
            }

            Events.EventManager.TriggerEvent(Events.GameEventTypes.StatsRecalculated, new EventArgs());

        }

        /// <summary>
        /// Класс заглушка. Имитирует хранилище очков, распределенных игроком.
        /// </summary>
        private static class PlayerStatsAssigment
        {
            public static Dictionary<Stats, int> GetStatsAssigment()
            {
                var dictionary = new Dictionary<Stats, int>();

                foreach (var stat in (Stats[])Enum.GetValues(typeof(Stats)))
                    dictionary.Add(stat, 3);

                return dictionary;
            }
        }
    }
}