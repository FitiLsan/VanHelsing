using UnityEngine;


namespace Models
{
    public abstract class NewNPCModel : ScriptableObject
    {
        #region PrivateData

        [Tooltip("ID")]
        [SerializeField] private int _npcID;

        [Tooltip("Name (role) of the NPC")]
        [SerializeField] private string _npcName;

        [Tooltip("Prefab for this NPC")]
        [SerializeField] private GameObject _npcPrefab;   

        [Tooltip("Base walk speed")]
        [SerializeField] private float _baseWalkSpeed;

        [Tooltip("Base run speed")]
        [SerializeField] private float _baseRunSpeed;

        [Tooltip("Is this NPC Dead")]
        [SerializeField] private bool _isDead;

        #endregion


        #region Fields

        [Tooltip("Day navigation points")]
        [SerializeField] public NPCPoint[] _dayNavigationPoints;

        [Tooltip("Night navigation points")]
        [SerializeField] public NPCPoint[] _nightNavigationPoints;

        [Tooltip("Point where npc escapes if it is dangerous")]
        [SerializeField] public NPCPoint _safePlacePoint;

        [Tooltip("Current point index")]
        [SerializeField] public int _currentPointIndex;

        [Tooltip("Position of this NPC")]
        [SerializeField] public Vector3 _prefabPosition;

        [Tooltip("Is this NPC running")]
        [SerializeField] public bool _isRunning;

        [Tooltip("Can this NPC Die")]
        [SerializeField] public bool _canDie;

        [Tooltip("Can this NPC be an enemy")]
        [SerializeField] public bool _canBeEnemy;

        [Tooltip("Is this NPC an enemy")]
        [SerializeField] public bool _isEnemy;

        [Tooltip("Is this NPC interactable")]
        [SerializeField] public bool _isInteractable;

        [Tooltip("Is this NPC escaping")]
        [SerializeField] public bool _isEscaping;

        [Tooltip("Is this NPC on one of the points")]
        [SerializeField] public bool _isOnPoint;

        #endregion


        #region Properties

        public GameObject NPCPrefab => _npcPrefab;

        public Vector3 NPCPosition => _prefabPosition;

        public string Name => _npcName;

        public int ID => _npcID;
        public int CurrentPointIndex => _currentPointIndex;

        public float BaseWalkSpeed => _baseWalkSpeed;
        public float BaseRunSpeed => _baseRunSpeed;

        public bool IsDead => _isDead;
        public bool CanDie => _canDie;
        public bool CanBeEnemy => _canBeEnemy;
        public bool IsEnemy => _isEnemy;
        public bool IsEscaping => _isEscaping;
        public bool IsInteractabe => _isInteractable;
        public bool IsOnPoint => _isOnPoint;
        public bool IsRunning => _isRunning;

        #endregion


        #region Methods

        public abstract void OnAwake();

        public abstract void Tick();

        public abstract void Init(GameObject prefab);

        public abstract void OnTriggerEnter(Collider other);

        public abstract void OnTriggerStay(Collider other);

        public abstract void OnTriggerExit(Collider other);

        public abstract void OnDayBegin();

        public abstract void OnNightBegin();

        public abstract void OnDangerAppear();

        public abstract void OnDangerEnd();

        #endregion
    }
}

