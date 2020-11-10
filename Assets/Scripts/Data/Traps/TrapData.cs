using UnityEngine;


namespace BeastHunter
{
    public abstract class TrapData : ScriptableObject
    {
        #region Fields

        [SerializeField] private TrapsEnum _trapType;
        [SerializeField] private TrapStruct _trapStruct;
        [SerializeField] private Sprite _trapImage;

        [SerializeField] private int _trapsAmount;
        [SerializeField] private string _animationName;

        [Range(0f, 90f)]
        [SerializeField] float _maximalGroundAngle;

        #endregion


        #region Properties

        protected TrapModel CurrentTrapModel { get; private set; }
        protected GameContext Context { get; private set; }

        public TrapsEnum TrapType => _trapType;
        public TrapStruct TrapStruct => _trapStruct;
        public Sprite TrapImage => _trapImage;

        public int TrapsAmount
        {
            get
            {
                return _trapsAmount;
            }
            protected set
            {
                _trapsAmount = value;
            }
        }

        public string AnimationName => _animationName;
        public float MaximalGroundAngle => _maximalGroundAngle;

        #endregion


        #region Methods

        public virtual void Place(GameContext context, TrapModel trapModel)
        {
            Context = context;
            CurrentTrapModel = trapModel;
        }

        public abstract bool OnTriggerFilter(Collider enteredCollider);

        public abstract void OnTriggerEnterSomething(ITrigger trapTrigger, Collider enteredCollider);

        #endregion
    }
}
