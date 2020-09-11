using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/TrapData", order = 0)]
    public sealed class TrapData : ScriptableObject
    {
        #region Fields

        public TrapsEnum TrapType;
        public TrapStruct TrapStruct;

        #endregion
    }
}
