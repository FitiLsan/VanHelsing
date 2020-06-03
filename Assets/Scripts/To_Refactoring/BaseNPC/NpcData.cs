using UnityEngine;

namespace BeastHunter
{
    public abstract class NpcData : ScriptableObject
    {
        #region Fields

        public GameObject Prefab;

        #endregion


        #region Methods

        public virtual void Do(string how)
        {
            Debug.Log("I did something " + how);
        }

        #endregion
    }

}
