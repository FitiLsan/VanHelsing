using UnityEngine;


namespace BeastHunter
{
    public sealed class InputController : IAwake, IUpdate
    {
        #region Properties

        private readonly GameContext _context;
        private CharacterModel _characterModel;
        private InputModel _inputModel;
        private InputStruct _inputStruct;

        #endregion


        #region ClassLifeCycle

        public InputController(GameContext context)
        {
            _context = context;
            _inputModel = new InputModel();
            _context.InputModel = _inputModel;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            _characterModel = _context.CharacterModel;
            _inputStruct._inputAxisX = 0;
            _inputStruct._inputAxisY = 0;
            _inputStruct._inputTotalAxisX = 0;
            _inputStruct._inputTotalAxisY = 0;

            _inputStruct._isInputJump = false;
            _inputStruct._isInputRun = false;
            _inputStruct._isInputDodge = false;
            _inputStruct._isInputBattleExit = false;
            _inputStruct._isInputTargetLock = false;
            _inputStruct._isInputAttackLeft = false;
            _inputStruct._isInputAttackRight = false;
            _inputStruct._isInputUse = false;
            _inputStruct._isInputDance = false;
        }

        #endregion


        #region Updating

        public void Updating()
        {
            GetInput();
            _inputModel.inputStruct = _inputStruct;
        }

        #endregion


        #region Methods

        public void GetInput()
        {
            _inputStruct._inputAxisX = Input.GetAxis("Horizontal");
            _inputStruct._inputAxisY = Input.GetAxis("Vertical");
            _inputStruct._isInputRun = Input.GetButton("Sprint");
            _inputStruct._isInputJump = Input.GetButtonDown("Jump");
            _inputStruct._isInputBattleExit = Input.GetButtonDown("Battle Exit");
            _inputStruct._isInputDodge = Input.GetButtonDown("Dodge");
            _inputStruct._isInputTargetLock = Input.GetButtonDown("Target lock");
            _inputStruct._isInputAttackLeft = Input.GetButtonDown("AttackLeft");
            _inputStruct._isInputAttackRight = Input.GetButtonDown("AttackRight");
            _inputStruct._isInputUse = Input.GetButtonDown("Use");
            _inputStruct._isInputDance = Input.GetButtonDown("Dance");

            CheckAxisTotal();
            CheckEvents();
        }

        private void CheckAxisTotal()
        {
            _inputStruct._inputTotalAxisX = _inputStruct._inputAxisX > 0 ? 1 : _inputStruct._inputAxisX < 0 ? -1 : 0;
            _inputStruct._inputTotalAxisY = _inputStruct._inputAxisY > 0 ? 1 : _inputStruct._inputAxisY < 0 ? -1 : 0;
        }

        private void CheckEvents()
        {
            if (_inputStruct._isInputJump) _inputModel.OnJump?.Invoke();
            if (_inputStruct._isInputDodge) _inputModel.OnDodge?.Invoke();
            if (_inputStruct._isInputAttackLeft) _inputModel.OnAttackLeft?.Invoke();
            if (_inputStruct._isInputAttackRight) _inputModel.OnAttackRight?.Invoke();
            if (_inputStruct._isInputTargetLock) _inputModel.OnTargetLock?.Invoke();
            if (_inputStruct._isInputBattleExit) _inputModel.OnBattleExit?.Invoke();
            if (_inputStruct._isInputUse) _inputModel.OnUse?.Invoke();
            if (_inputStruct._isInputDance) _inputModel.OnDance?.Invoke();
        }

        #endregion
    }
}


