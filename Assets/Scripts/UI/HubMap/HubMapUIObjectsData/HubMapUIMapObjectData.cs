using UnityEngine;

namespace BeastHunter
{
    public abstract class HubMapUIMapObjectData : ScriptableObject
    {
        #region Fields

        [Header("Map object data")]
        [SerializeField] private bool _isBlockedAtStart;
        [SerializeField] private string _name;
        [SerializeField] [TextArea(3, 10)] private string _description;

        #endregion


        #region Properties

        public bool IsBlockedAtStart => _isBlockedAtStart;
        public string Name => _name;
        public string Description => _description;

        #endregion


        #region Methods

        public abstract HubMapUIMapObjectType GetMapObjectType();

        #endregion
    }
}
