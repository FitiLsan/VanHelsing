using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;     
        private InputModel _inputModel;
        private Services _services;

        private CharacterModel _characterModel;
        private CharacterStateMachine _stateMachine;

        #endregion


        #region ClassLifeCycle

        public CharacterController(GameContext context)
        {
            _context = context;
            _inputModel = _context.InputModel;
            _services = Services.SharedInstance;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _characterModel = _context.CharacterModel;
            _stateMachine = new CharacterStateMachine(_context);
            _stateMachine.OnAwake();
            _stateMachine.SetStartState(_stateMachine.CharacterStates[CharacterStatesEnum.Idle]);          
        }

        #endregion


        #region Updating

        public void Updating()
        {
            if (!_characterModel.IsDead)
            {
                _stateMachine.Execute();
            } 
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            _stateMachine.OnTearDown();
        }

        #endregion
    }
}

