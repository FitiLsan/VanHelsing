using System;

namespace Models.ConditionsAndActions.Helpers
{
    /// <summary>
    ///     Представляет данные для события IPropertyChanged.StatusChanged
    /// </summary>
    public class ConditionArgs : EventArgs
    {
        /// <summary>
        ///     Создает новый экземпляр класса ConditionArgs
        /// </summary>
        /// <param name="Name">Название измененного статуса</param>
        /// <param name="Status">Текущее значение измененного статуса</param>
        public ConditionArgs(string Name, bool Status)
        {
            ConditionName = Name;
            ConditionStatus = Status;
        }

        /// <summary>
        ///     Возвращает название изменного статуса
        /// </summary>
        public string ConditionName { get; }

        /// <summary>
        ///     Возвращает значение измененного статуса
        /// </summary>
        public bool ConditionStatus { get; }
    }
}