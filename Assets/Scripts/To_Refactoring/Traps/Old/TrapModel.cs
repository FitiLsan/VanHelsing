using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class TrapModel
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
            Debug.Log(Trap.transform.position);
        }

        #endregion
    }
}

