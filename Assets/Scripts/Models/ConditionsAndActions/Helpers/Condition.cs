using Models.ConditionsAndActions.Helpers.Components;
using Models.ConditionsAndActions.Helpers.Interfaces;

namespace Models.ConditionsAndActions.Helpers
{
    /// <summary>
    ///     Представляет отдельный статус персонажа
    /// </summary>
    public class Condition : IPropertyChanged
    {
        private bool Status;

        public Condition(string Name, bool Status)
        {
            GetName = Name;
            this.Status = Status;
        }

        /// <summary>
        ///     Получить название статуса
        /// </summary>
        public string GetName { get; }

        /// <summary>
        ///     Получить\Задать статус состояния.
        /// </summary>
        public bool StatusChanged
        {
            get => Status;

            set
            {
                StatusChangedEvent.Invoke(new ConditionArgs(GetName, value));
                Status = value;
            }
        }

        /// <summary>
        ///     Событие возникает при изменении Состояния
        /// </summary>
        public event StatusProperty StatusChangedEvent;
    }
}