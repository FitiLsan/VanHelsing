using UnityEngine;


namespace BeastHunter
{
    public sealed class InputController : IAwake, ITearDown
    {
        #region Properties

        private readonly GameContext _context;
        private InputModel _inputModel;

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
            _inputModel.MainInput.Enable();
            _inputModel.MainInput.Player.Movement.performed += ctx => GetInputMovement(ctx.ReadValue<Vector2>());
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _inputModel.MainInput.Disable();
        }

        #endregion


        #region Methods

        public void GetInputMovement(Vector2 movementVector)
        {
            _inputModel.InputAxisX = movementVector.x;
            _inputModel.InputAxisY = movementVector.y;
            CheckAxisTotal();
            Debug.LogError(movementVector);
        }

        private void CheckAxisTotal()
        {
            _inputModel.InputTotalAxisX = _inputModel.InputAxisX > 0 ? 1 : _inputModel.InputAxisX < 0 ? -1 : 0;
            _inputModel.InputTotalAxisY = _inputModel.InputAxisY > 0 ? 1 : _inputModel.InputAxisY < 0 ? -1 : 0;
        }

        #endregion
    }
}


