using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "HubUIData", menuName = "CreateData/HubUIData/MainData", order = 0)]
    public class HubUIData : ScriptableObject
    {
        #region Fields

        [Header("STARTING GAME DATA")]
        [SerializeField] private GameData _startGameData;

        [Space(20, order = 1), Header("OBJECTS POOLS", order = 2)]
        [SerializeField] private QuestSO[] _questsSOPool;
        [SerializeField] private HuntingQuestSO[] _huntingQuestsSOPool;
        [SerializeField] private BossDataSO[] _bossesSOPool;
        [SerializeField] private ItemSOPools _itemSOPools;

        [Space(20, order = 1), Header("GLOBAL DATA", order = 2)]
        [SerializeField] private GameTimeGlobalData _gameTimeGlobalData;
        [SerializeField] private CharactersGlobalData _charactersGlobalData;
        [SerializeField] private int _citiesShopsSlotsAmount;

        [Space(20, order = 1), Header("HUB UI DATA", order = 2)]
        [SerializeField] private GameObject _messageWindowPrefab;
        [SerializeField] private MapRoomUIData _mapData;
        [SerializeField] private QuestRoomUIData _questRoomData;
        [SerializeField] private WorkRoomUIData _workRoomData;

        #endregion


        #region Properties

        public MapRoomUIData MapData => _mapData;
        public QuestRoomUIData QuestRoomData => _questRoomData;
        public WorkRoomUIData WorkRoomData => _workRoomData;
        public CharactersGlobalData CharactersGlobalData => _charactersGlobalData;
        public GameData StartGameDataStruct => _startGameData;
        public GameTimeGlobalData GameTimeGlobalData => _gameTimeGlobalData;
        public GameObject MessageWindowPrefab => _messageWindowPrefab;
        public int CitiesShopsSlotsAmount => _citiesShopsSlotsAmount;
        public ItemSOPools ItemDataPools => _itemSOPools;
        public QuestSO[] QuestsPool => (QuestSO[])_questsSOPool?.Clone();
        public BossDataSO[] BossesDataPool => (BossDataSO[])_bossesSOPool?.Clone();
        public HuntingQuestSO[] HuntingQuestsDataPool => (HuntingQuestSO[])_huntingQuestsSOPool?.Clone();

        #endregion
    }
}
