using UnityEngine;

namespace BeastHunter
{
    public abstract class BaseInteractiveObjectModel
    {
        #region Properties

        public BaseInteractiveObjectData InteractiveObjectData { get; protected set; }
        public bool IsInteractive { get; set; }
        public bool IsNeedControl { get; set; }
        public bool IsActivated { get; set; }
        public GameObject Prefab { get; protected set; }

        #endregion
    }
}
