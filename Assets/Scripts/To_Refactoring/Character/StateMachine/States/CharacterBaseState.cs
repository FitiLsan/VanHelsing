﻿namespace BeastHunter
{
    public abstract class CharacterBaseState
    {
        #region Fields

        protected readonly InputModel _inputModel;
        protected readonly CharacterModel _characterModel;
        protected CharacterAnimationController _animationController;
        protected CharacterStateMachine _stateMachine;

        #endregion


        #region Properties

        public CharacterBaseState NextState { get; set; }
        public StateType Type { get; protected set; }
        public bool IsTargeting { get; protected set; }
        public bool IsAttacking { get; protected set; }
        public bool CanExit { get; protected set; }
        public bool CanBeOverriden { get; protected set; }

        #endregion


        #region ClassLifeCycle

        public CharacterBaseState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController, 
            CharacterStateMachine stateMachine)
        {
            _characterModel = characterModel;
            _inputModel = inputModel;
            _animationController = animationController;
            _stateMachine = stateMachine;
        }

        #endregion


        #region Methods

        public abstract void Initialize();

        public abstract void Execute();

        public abstract void OnExit();

        public abstract void OnTearDown();

        #endregion
    }
}
