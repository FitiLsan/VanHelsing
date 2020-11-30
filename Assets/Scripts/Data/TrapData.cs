﻿using UnityEngine;


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

        #endregion


        #region Properties

        protected TrapModel _currentTrapModel { get; private set; }
        protected GameContext _context { get; private set; }

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

        #endregion


        #region Methods

        public virtual void Place(GameContext context, TrapModel trapModel)
        {
            _context = context;
            _currentTrapModel = trapModel;
        }

        public abstract bool OnTriggerFilter(Collider enteredCollider);

        public abstract void OnTriggerEnterSomething(ITrigger enteredITrigger, Collider enteredCollider);

        #endregion
    }
}