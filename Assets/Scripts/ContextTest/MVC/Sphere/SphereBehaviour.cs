using UnityEngine;


namespace BeastHunter
{
    public sealed class SphereBehaviour : MonoBehaviour
    {
        #region Properties

        public SphereCollider Collider { get; private set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Collider = GetComponent<SphereCollider>();
        }

        #endregion
    }
}

