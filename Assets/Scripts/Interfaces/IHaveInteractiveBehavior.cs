using UnityEngine;


namespace BeastHunter
{
    public interface IHaveInteractiveBehavior
    {
        #region Properties

        InteractableObjectBehavior InteractiveBehavior { get; }

        #endregion
    }
}

