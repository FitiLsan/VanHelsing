using UnityEngine;
using System;

namespace BeastHunter
{
    public abstract class WeaponData : ScriptableObject
    {
        #region Fields

        public Action<ITrigger, Collider> OnHit;

        public WeaponHandType HandType;
        public WeaponType Type;
        public HandsEnum InWhichHand;

        public Sprite WeaponImage;
        public AttackModel[] SimpleAttacks;
        public AttackModel[] SpecialAttacks;
        public AttackModel CurrentAttack;

        public string WeaponName;
        public string SimpleAttackAnimationPrefix;
        public string SpecialAttackAnimationPrefix;
        public string StrafeAndDodgePostfix;

        public string GettingAnimationName;
        public string RemovingAnimationName;

        [Tooltip("Time needed to get the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TotalTimeToGet;

        [Tooltip("Time before character grabs the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeBeforeGrab;

        [Tooltip("Time needed to remove the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TotalTimeToRemove;

        [Tooltip("Time before character lets go the weapon between 1 and 10.")]
        [Range(0.0f, 10.0f)]
        public float TimeBeforeLetGo;

        private int _currentSimpleAttackIndex;
        private int _currentSpecialAttackIndex;

        #endregion


        #region Properties

        public bool IsInHands { get; private set; }

        public int CurrentSimpleAttackIndex
        {
            get
            {
                return _currentSimpleAttackIndex;
            }
            private set
            {
                if(value >= SimpleAttacks.Length)
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
                if (value >= SpecialAttacks.Length)
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

