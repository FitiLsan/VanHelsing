using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateModel/Boss", order = 1))]
    public class BossData : ScriptableObject
    {
        #region Fields

        public GameObject prefab;

        #endregion
    }
}
