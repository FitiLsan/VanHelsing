using UnityEngine;


namespace BeastHunter
{
    public sealed class InputController : IAwake, IUpdate, ITearDown
    {
        #region Properties

        private readonly GameContext _context;
        private readonly MainInput _mainInput;
        private InputModel _inputModel;

        #endregion

        
        #region ClassLifeCycle

        public InputController(GameContext context)
        {
            _context = context;
            _mainInput = new MainInput();
            _inputModel = new InputModel();
            _context.InputModel = _inputModel;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _mainInput.Enable();
            _mainInput.Player.Movement.performed += ctx => GetInputMovement(ctx.ReadValue<Vector2>());
            _mainInput.Player.Run.performed += ctx => GetInputRun(ctx.ReadValueAsButton());
            _mainInput.Player.Aim.performed += ctx => _inputModel.OnAim?.Invoke();
            _mainInput.Player.Attack.performed += ctx => _inputModel.OnAttack?.Invoke();
            _mainInput.Player.Bestiary.performed += ctx => _inputModel.OnBestiary?.Invoke();
            _mainInput.Player.ButtonsInfo.performed += ctx => _inputModel.OnButtonsInfo?.Invoke();
            _mainInput.Player.Cancel.performed += ctx => _inputModel.OnPressCancel?.Invoke();
            _mainInput.Player.Enter.performed += ctx => _inputModel.OnPressEnter?.Invoke();
            _mainInput.Player.Jump.performed += ctx => _inputModel.OnJump?.Invoke();
            _mainInput.Player.NumberFour.performed += ctx => _inputModel.OnPressNumberFour?.Invoke();
            _mainInput.Player.NumberThree.performed += ctx => _inputModel.OnPressNumberThree?.Invoke();
            _mainInput.Player.NumberTwo.performed += ctx => _inputModel.OnPressNumberTwo?.Invoke();
            _mainInput.Player.NumberOne.performed += ctx => _inputModel.OnPressNumberOne?.Invoke();
            _mainInput.Player.SneakSlide.performed += ctx => _inputModel.OnSneakSlide?.Invoke();
            _mainInput.Player.Use.performed += ctx => _inputModel.OnUse?.Invoke();
            _mainInput.Player.WeaponRemove.performed += ctx => _inputModel.OnRemoveWeapon?.Invoke();
            _mainInput.Player.WeaponWheel.performed += ctx => _inputModel.OnWeaponWheel?.Invoke(ctx.ReadValueAsButton());
            _mainInput.Player.Bestiary.performed += x => Debug.LogError("pressed ");
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            GetMouseInput(_mainInput.Player.MouseLook.ReadValue<Vector2>());
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _mainInput.Player.Movement.performed -= ctx => GetInputMovement(ctx.ReadValue<Vector2>());
            _mainInput.Player.Run.performed -= ctx => GetInputRun(ctx.ReadValueAsButton());
            _mainInput.Disable();
        }

        #endregion


        #region Methods

        private void GetInputMovement(Vector2 movementVector)
        {
            _inputModel.InputAxisX = movementVector.x;
            _inputModel.InputAxisY = movementVector.y;
            CheckAxisTotal();
        }

        private void GetMouseInput(Vector2 mouseMovementVector)
        {
            _inputModel.MouseInputX = mouseMovementVector.x;
            _inputModel.MouseInputY = mouseMovementVector.y;
        }

        private void GetInputRun(bool isPressed)
        {
            _inputModel.IsInputRun = isPressed;
        }

        private void CheckAxisTotal()
        {
            _inputModel.InputTotalAxisX = _inputModel.InputAxisX > 0 ? 1 : _inputModel.InputAxisX < 0 ? -1 : 0;
            _inputModel.InputTotalAxisY = _inputModel.InputAxisY > 0 ? 1 : _inputModel.InputAxisY < 0 ? -1 : 0;
            _inputModel.IsInputMove = _inputModel.InputTotalAxisX != 0 || _inputModel.InputTotalAxisY != 0;
        }

        #endregion
    }
}


