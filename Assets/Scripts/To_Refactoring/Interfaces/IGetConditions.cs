using Models.ConditionsAndActions;

namespace Interfaces
{
    /// <summary>
    ///     Позволяет добавлять Статусы
    /// </summary>
    public interface IGetConditions
    {
        void ApplyCondition(params CurrentCondition[] Characteristics);
    }
}