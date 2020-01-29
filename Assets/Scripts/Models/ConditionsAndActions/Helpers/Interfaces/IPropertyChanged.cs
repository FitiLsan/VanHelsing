using Models.ConditionsAndActions.Helpers.Components;

namespace Models.ConditionsAndActions.Helpers.Interfaces
{
    public interface IPropertyChanged
    {
        event StatusProperty StatusChangedEvent;
    }
}