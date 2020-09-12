using UnityEngine;


namespace BeastHunter
{
    public sealed class TrapModel
    {
        #region Fields

        public TrapData TrapData;
        public TrapStruct TrapStruct;
        public GameObject Trap;
        public Transform AttackPlace;

        #endregion


        #region ClassLifeCycle

        public TrapModel(GameObject prefab, TrapData trapData)
        {
            TrapData = trapData;
            TrapStruct = trapData.TrapStruct;
            Trap = prefab;
        }

        #endregion


        #region Metods

        public void Execute()
        {
        }

        #endregion
    }
}

