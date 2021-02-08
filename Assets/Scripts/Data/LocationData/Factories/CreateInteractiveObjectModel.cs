using UnityEngine;


namespace BeastHunter
{
    public abstract class CreateInteractiveObjectModel
    {
        #region Methods

        public abstract bool CanCreateModel(InteractiveObjectType type);

        public abstract SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data);

        #endregion
    }
}

