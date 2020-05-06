using UnityEngine;


namespace BeastHunter
{
    public sealed class NpcDialogue : MonoBehaviour, IGetNpcInfo
    {
        #region Fields
       
        public Vector3 NpcPos;
        public int NpcID;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            NpcPos = transform.position;
        }

        #endregion


        #region Methods

        public (int, Vector3) GetInfo()
        {
            return (NpcID, NpcPos);
        }

        #endregion

    }
}