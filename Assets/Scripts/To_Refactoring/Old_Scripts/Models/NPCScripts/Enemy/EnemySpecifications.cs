using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    [CreateAssetMenu]
    public class EnemySpecifications : ScriptableObject
    {
        private Dictionary<string, bool> AllConditions;

        /// <summary>
        ///     Возвращает список доступных статусов для персонажа
        /// </summary>
        /// <returns></returns>
        public List<string> GetCharacterConditionsList()
        {
            AllConditions = new Dictionary<string, bool>();

            #region Добавляем все состояния в словарь

            AllConditions.Add("Bleeding", Bleeding);
            AllConditions.Add("Poisoned", Poison);
            AllConditions.Add("Weakness", Weakness);
            AllConditions.Add("KnokedDown", KnokedDown);
            AllConditions.Add("Blinded", Blinding);
            AllConditions.Add("Immobilized", Immobilizing);
            AllConditions.Add("Slowed", Slowing);

            #endregion

            return (from c in AllConditions where c.Value select c.Key).ToList();
        }

        #region Основные характеристики персонажа

        [SerializeField] private string type = string.Empty;
        public string Type => type;

        [SerializeField] private float hp;

        internal float HP
        {
            get => hp;
            set => hp = value;
        }

        [SerializeField] private float speed;
        public float Speed => speed;

        [SerializeField] private float runSpeed;
        public float RunSpeed => runSpeed;

        [SerializeField] private bool rangeAttack;
        public bool RangeAttack => rangeAttack;

        [SerializeField] private float rangeDamage;
        public float RangeDamage => rangeDamage;

        [SerializeField] private float rangeDistance;
        public float RangeDistance => rangeDistance;

        [SerializeField] private float rangeAccuracy;
        public float RangeAccuracy => rangeAccuracy;

        [SerializeField] private float shootSpeed;
        public float ShootSpeed => shootSpeed;

        [SerializeField] private bool meleeAttack;
        public bool MeleeAttack => meleeAttack;

        [SerializeField] private float meleeDamage;
        public float MeleeDamage => meleeDamage;

        [SerializeField] private float hitSpeed;
        public float HitSpeed => hitSpeed;

        [SerializeField] private float meleeDistance;
        public float MeleeDistance => meleeDistance;

        [SerializeField] private float viewDistance;
        public float ViewDistance => viewDistance;

        [SerializeField] private float patrolDistance;
        public float PatrolDistance => patrolDistance;

        [SerializeField] private float chasingTime;
        public float ChasingTime => chasingTime;

        #endregion

        #region Характеристикик состояний персонажа

        [Header("Состояния наносящие урон здоровью")] [Tooltip("Кровотечение")] [SerializeField]
        private bool Bleeding;

        [Tooltip("Урон от Кровотечение")] [SerializeField]
        private float _BleedingDamage;

        public float BleedingDamage => _BleedingDamage;

        [Tooltip("Время Кровотечение")] [SerializeField]
        private float _BleedingTime;

        public float BleedingTime => _BleedingTime;

        [Tooltip("Отравление")] [SerializeField]
        private bool Poison;

        [Tooltip("Урон от Отравления")] [SerializeField]
        private float _PoisonDamage;

        public float PoisonDamage => _PoisonDamage;

        [Tooltip("Время Отравления")] [SerializeField]
        private float _PoisonTime;

        public float PoisonTime => _PoisonTime;

        [Header("Состояния уменьшающие урон")] [Tooltip("Ослабление")] [SerializeField]
        private bool Weakness;

        [Tooltip("Процент уменьшения физического урон от Ослабления")] [SerializeField] [Range(0f, 1f)]
        private float _WeaknessDamageReduce;

        public float WeaknessDamageReduce => _WeaknessDamageReduce;

        [Tooltip("Время ослабления")] [SerializeField]
        private float _WeaknessTime;

        public float WeaknessTime => _WeaknessTime;

        [Header("Состояния контроля")] [Tooltip("Нокдаун")] [SerializeField]
        private bool KnokedDown;

        [Tooltip("Время Нокдауна")] [SerializeField]
        private float _KnokedDownTime;

        public float KnokedDownTime => _KnokedDownTime;

        [Tooltip("Нокдаун")] [SerializeField] private bool Blinding;

        [Tooltip("Время Ослепления")] [SerializeField]
        private float _BlindingTime;

        public float BlindingTime => _BlindingTime;

        [Tooltip("Иммобилизации")] [SerializeField]
        private bool Immobilizing;

        [Tooltip("Время Иммобилизации")] [SerializeField]
        private float _ImmobilizingTime;

        public float ImmobilizingTime => _ImmobilizingTime;

        [Tooltip("Замедления")] [SerializeField]
        private bool Slowing;

        [Tooltip("Скорость замедленного персонажа")] [SerializeField]
        private float _SlowSpeed;

        [Tooltip("Время Замедления")] [SerializeField]
        private float _SlowingTime;

        public float SlowingTime => _SlowingTime;
        public float SlowSpeed => _SlowSpeed;

        #endregion
    }
}