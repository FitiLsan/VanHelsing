using UnityEngine;
using System;


namespace BeastHunter
{
    public abstract class WeaponData : ScriptableObject
    {
        #region Fields

        public Action<ITrigger, Collider> OnHit;

        [SerializeField] protected WeaponHandType _handType;
        [SerializeField] protected WeaponType _type;
        [SerializeField] protected HandsEnum _inWhichHand;

        [SerializeField] protected Sprite _weaponImage;
        [SerializeField] private Sound _gettingSound;
        [SerializeField] private Sound _removingSound;
        [SerializeField] protected AttackModel[] _simpleAttacks;
        [SerializeField] protected AttackModel[] _specialAttacks;

        [SerializeField] protected string _weaponName;
        [SerializeField] protected string _simpleAttackAnimationPrefix;
        [SerializeField] protected string _specialAttackAnimationPrefix;
        [SerializeField] protected string _strafeAnimationPostfix;
        [SerializeField] protected string _dodgeAnimationPostfix;
        [SerializeField] protected string _gettingAnimationPostfix;
        [SerializeField] protected string _holdingAnimationPostfix;
        [SerializeField] protected string _removingAnimationPostfix;

        [Tooltip("Time needed to get the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] protected float _totalTimeToGet;

        [Tooltip("Time before character grabs the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] protected float _timeBeforeGrab;

        [Tooltip("Time needed to remove the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] protected float _totalTimeToRemove;

        [Tooltip("Time before character lets go the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] protected float _timeBeforeLetGo;

        protected GameContext _context;
        private int _currentSimpleAttackIndex = 0;
        private int _currentSpecialAttackIndex = 0;

        #endregion


        #region Properties

        public WeaponHandType HandType => _handType;
        public WeaponType Type => _type;
        public HandsEnum InWhichHand => _inWhichHand;

        public Sprite WeaponImage => _weaponImage;
        public Sound GettingSound => _gettingSound;
        public Sound RemovingSound => _removingSound;
        public AttackModel[] SimpleAttacks => _simpleAttacks;
        public AttackModel[] SpecialAttacks => _specialAttacks;
        public AttackModel CurrentAttack { get; private set; }

        public string WeaponName => _weaponName;
        public string SimpleAttackAnimationPrefix => _simpleAttackAnimationPrefix;
        public string SpecialAttackAnimationPrefix => _specialAttackAnimationPrefix;
        public string StrafeAnimationPostfix => _strafeAnimationPostfix;
        public string DodgeAnimationPostfix => _dodgeAnimationPostfix;
        public string GettingAnimationPostfix => _gettingAnimationPostfix;
        public string HoldingAnimationPostfix => _holdingAnimationPostfix;
        public string RemovingAnimationPostfix => _removingAnimationPostfix;

        public float TotalTimeToGet => _totalTimeToGet;
        public float TimeBeforeGrab => _timeBeforeGrab;
        public float TotalTImeToRemove => _totalTimeToRemove;
        public float TimeBeforeLetGo => _timeBeforeLetGo;

        public bool IsInHands { get; private set; }
        public bool IsCurrentAttackSpecial { get; private set; }

        public int CurrentSimpleAttackIndex
        {
            get
            {
                return _currentSimpleAttackIndex;
            }
            private set
            {
                if(value >= _simpleAttacks.Length)
                {
                    _currentSimpleAttackIndex = 0;
                }
                else
                {
                    _currentSimpleAttackIndex = value;
                }
            }
        }

        public int CurrentSpecialAttackIndex
        {
            get
            {
                return _currentSpecialAttackIndex;
            }
            private set
            {
                if (value >= _specialAttacks.Length)
                {
                    _currentSpecialAttackIndex = 0;
                }
                else
                {
                    _currentSpecialAttackIndex = value;
                }
            }
        }

        protected Transform BodyTransform { get; private set; }

        #endregion


        #region Methods

        public virtual void Init(GameContext context)
        {
            if (_context == null) _context = context;
        }

        public virtual void MakeSimpleAttack(out int currentAttackIntex, Transform bodyTransform)
        {
            BodyTransform = bodyTransform;
            currentAttackIntex = CurrentSimpleAttackIndex;
            CurrentAttack = SimpleAttacks[CurrentSimpleAttackIndex];
            CurrentSimpleAttackIndex++;
        }

        public virtual void MakeSpecialAttack(out int currentAttackIntex, Transform bodyTransform)
        {
            BodyTransform = bodyTransform;
            currentAttackIntex = CurrentSpecialAttackIndex;
            CurrentAttack = SpecialAttacks[CurrentSpecialAttackIndex];
            CurrentSpecialAttackIndex++;
        }

        public virtual void TakeWeapon()
        {
            IsInHands = true;
        }

        public virtual void LetGoWeapon()
        {
            IsInHands = false;
        }

        #endregion
    }
}

