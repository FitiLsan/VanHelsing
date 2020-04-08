using UnityEngine;

namespace BeastHunter
{
    public class NpcDialogue : MonoBehaviour, IGetNpcInfo
    {
        #region Fields
       
        public Vector3 npcPos;
        public int _npcID;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            npcPos = gameObject.transform.position;
        }

        #endregion

        #region Methods

        public (int, Vector3) GetInfo()
        {
            var turple = (_npcID, npcPos);
            return turple;
        }

        #endregion

    }
}