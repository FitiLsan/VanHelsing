﻿using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "Character/TrapData", order = 0)]
    public abstract class TrapData : ScriptableObject
    {
        #region Fields

        [SerializeField] private TrapsEnum _trapType;
        [SerializeField] private TrapStruct _trapStruct;
        [SerializeField] private Sprite _trapImage;

        [SerializeField] private int _trapsAmount;
        [SerializeField] private int _trapsAmountCurrent;
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


        public int TrapsAmount { get { return _trapsAmount; } protected set { _trapsAmount = value; } }
        public int TrapsAmountCurrent { get { return _trapsAmountCurrent; } set { _trapsAmountCurrent = value; } }


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
