using System.Linq;
using UnityEngine;

namespace Models.ConditionsAndActions
{
    /// <summary>
    ///     Содержит данные о статусе который может передать объект и шансах его срабатывания
    /// </summary>
    public struct CurrentCondition
    {
        public string Name { get; }

        public float Chance { get; }

        public CurrentCondition(string Name, float Chance)
        {
            this.Name = Name;
            this.Chance = Chance;
        }
    }

    /// <summary>
    ///     Содержит данные всех статусах, которые может передать объект и шансах их срабатывания
    /// </summary>
    [CreateAssetMenu]
    public class ItemConditionsChance : ScriptableObject
    {
        private CurrentCondition[] ItemConditions = new CurrentCondition[8];

        public void SetItemConditionsChance()
        {
            ItemConditions = new[]
            {
                new CurrentCondition("Bleeding", Bleeding),
                new CurrentCondition("Poisoned", Poisoned),
                new CurrentCondition("Weakness", Weak),
                new CurrentCondition("KnokedDown", KnokedDown),
                new CurrentCondition("Blinded", Blinded),
                new CurrentCondition("Immobilized", Immobilized),
                new CurrentCondition("Slowed", Slow)
            };
        }

        /// <summary>
        ///     Получить все активные статусы на объекте
        /// </summary>
        /// <returns></returns>
        public CurrentCondition[] GetCurrentItemConditions()
        {
            return (from Con in ItemConditions where Con.Chance != 0 select Con).ToArray();
        }

        #region Шансы статусов предмета

        [SerializeField] [Range(0f, 1f)] private float Slow;
        [SerializeField] [Range(0f, 1f)] private float Immobilized;
        [SerializeField] [Range(0f, 1f)] private float Blinded;
        [SerializeField] [Range(0f, 1f)] private float Bleeding;
        [SerializeField] [Range(0f, 1f)] private float KnokedDown;
        [SerializeField] [Range(0f, 1f)] private float Poisoned;
        [SerializeField] [Range(0f, 1f)] private float Weak;

        #endregion
    }
}