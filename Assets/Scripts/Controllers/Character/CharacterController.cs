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
        private VisualEffectController _visualEffectController;
        private EffectReactionController _effectReactionController;

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
            _visualEffectController = new VisualEffectController(_context, _characterModel);
            _visualEffectController.OnAwake();
            _effectReactionController = new EffectReactionController(_context, _characterModel);
            _effectReactionController.OnAwake();
           
            
        }

        #endregion


        #region Updating

        public void Updating()
        {
            if (!_characterModel.CurrentStats.BaseStats.IsDead)
            {
                _stateMachine.Execute();
                _visualEffectController.Execute();
            } 
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            _stateMachine.OnTearDown();
            _visualEffectController.OnTearDown();
        }

        #endregion
    }
}

