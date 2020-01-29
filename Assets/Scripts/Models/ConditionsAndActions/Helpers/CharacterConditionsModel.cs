using Models.NPCScripts.Enemy;

namespace Models.ConditionsAndActions.Helpers
{
    /// <summary>
    ///     Содержит данные о характеристиках статусов персонажа
    /// </summary>
    public struct CharacterConditionsSpecifications
    {
        public float BleedingDamage { get; }
        public float BleedingTime { get; }
        public float PoisonDamage { get; }
        public float PoisonTime { get; }
        public float WeaknessDamageReduce { get; }
        public float WeaknessTime { get; }
        public float KnokedDownTime { get; }
        public float BlindingTime { get; }
        public float ImmobilizingTime { get; }
        public float SlowingTime { get; }
        public float SlowSpeed { get; }

        /// <summary>
        ///     Создает модель данных о характеристиках персонажа
        /// </summary>
        /// <param name="Specifications">Все данные о персонаже</param>
        public CharacterConditionsSpecifications(EnemySpecifications Specifications)
        {
            BleedingDamage = Specifications.BleedingDamage;
            BleedingTime = Specifications.BleedingTime;
            PoisonDamage = Specifications.PoisonDamage;
            PoisonTime = Specifications.PoisonTime;
            WeaknessDamageReduce = Specifications.WeaknessDamageReduce;
            WeaknessTime = Specifications.WeaknessTime;
            KnokedDownTime = Specifications.KnokedDownTime;
            BlindingTime = Specifications.BlindingTime;
            ImmobilizingTime = Specifications.ImmobilizingTime;
            SlowingTime = Specifications.SlowingTime;
            SlowSpeed = Specifications.SlowSpeed;
        }
    }
}