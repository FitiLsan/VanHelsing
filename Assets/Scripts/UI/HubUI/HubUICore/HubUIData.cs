using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "HubUIData", menuName = "CreateData/HubUIData/MainData", order = 0)]
    public class HubUIData : ScriptableObject
    {
        #region Fields

        [Header("UI prefabs")]
        [SerializeField] private GameObject _messageWindowPrefab;

        [Header("GAME CONTENT SETTINGS")]
        public GameContentDataStruct ContextDataStruct;

        [Space(20, order = 1), Header("MAP DATA SETTINGS", order = 2)]
        public MapDataStruct MapDataStruct;

        #endregion


        #region Properties

        public GameObject MessageWindowPrefab => _messageWindowPrefab;

        #endregion
    }
}
