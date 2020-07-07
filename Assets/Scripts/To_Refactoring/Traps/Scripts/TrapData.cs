using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/TrapData", order = 0)]
    public class TrapData : ScriptableObject
    {
        #region Fields

        public TrapStruct TrapStruct;

        #endregion
    }
}
