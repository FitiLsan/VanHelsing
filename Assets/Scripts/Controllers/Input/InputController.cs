using UnityEngine;


namespace BeastHunter
{
    public sealed class InputController : IAwake, IUpdate
    {
        #region Properties

        private readonly GameContext _context;
        private CharacterModel _characterModel;
        private InputModel _inputModel;
        private EventManager _eventManager;

        #endregion

        
        #region ClassLifeCycle

        public InputController(GameContext context)
        {
            _context = context;
            _inputModel = new InputModel();
            _context.InputModel = _inputModel;
            _eventManager = Services.SharedInstance.EventManager;
        }

        #endregion


        #region OnAwake

        public void OnAwake()
        {
            _characterModel = _context.CharacterModel;
            _inputModel.MouseInputX = 0f;
            _inputModel.MouseInputY = 0f;
            _inputModel.InputAxisX = 0f;
            _inputModel.InputAxisY = 0f;
            _inputModel.InputTotalAxisX = 0f;
            _inputModel.InputTotalAxisY = 0f;
            _inputModel.IsInputMove = false;
            _inputModel.IsInputRun = false;

            _inputModel.inputOnButtonDown.Add("NO_BUTTON_1", InputEventTypes.MoveStart);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_2", InputEventTypes.MoveStop);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_3", InputEventTypes.RunStart);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_4", InputEventTypes.RunStop);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_5", InputEventTypes.WeaponWheelOpen);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_6", InputEventTypes.WeaponWheelClose);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_7", InputEventTypes.AimStart);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_8", InputEventTypes.AimEnd);

            _inputModel.inputOnButtonDown.Add("Jump", InputEventTypes.Jump);
            _inputModel.inputOnButtonDown.Add("Attack", InputEventTypes.Attack);
            _inputModel.inputOnButtonDown.Add("AimTarget", InputEventTypes.Aim);
            _inputModel.inputOnButtonDown.Add("Use", InputEventTypes.Use);
            _inputModel.inputOnButtonDown.Add("Cancel", InputEventTypes.Cancel);
            _inputModel.inputOnButtonDown.Add("NumberOne", InputEventTypes.NumberOne);
            _inputModel.inputOnButtonDown.Add("NumberTwo", InputEventTypes.NumberTwo);
            _inputModel.inputOnButtonDown.Add("NumberThree", InputEventTypes.NumberThree);
            _inputModel.inputOnButtonDown.Add("NumberFour", InputEventTypes.NumberFour);
            _inputModel.inputOnButtonDown.Add("Inventory", InputEventTypes.Inventory);
            _inputModel.inputOnButtonDown.Add("QuestJournal", InputEventTypes.QuestJournal);
            _inputModel.inputOnButtonDown.Add("Sneak", InputEventTypes.Sneak);
            _inputModel.inputOnButtonDown.Add("TimeSkip", InputEventTypes.TimeSkipMenu);
            _inputModel.inputOnButtonDown.Add("ButtonsInfo", InputEventTypes.ButtonsInfoMenu);
            _inputModel.inputOnButtonDown.Add("Bestiary", InputEventTypes.Bestiary);
            _inputModel.inputOnButtonDown.Add("WeaponRemove", InputEventTypes.WeaponRemove);
        }

        #endregion


        #region Updating

        public void Updating()
        {
            GetInput();
        }

        #endregion


        #region Methods

        public void GetInput()
        {
            _inputModel.MouseInputX = Input.GetAxis("Mouse X");
            _inputModel.MouseInputY = Input.GetAxis("Mouse Y");
            _inputModel.InputAxisX = Input.GetAxis("Horizontal");
            _inputModel.InputAxisY = Input.GetAxis("Vertical");
            _inputModel.IsInputMove = (_inputModel.InputAxisX != 0 || _inputModel.InputAxisY != 0) ? true : false;
            _inputModel.IsInputRun = Input.GetButton("Sprint");
            _inputModel.IsInputAim = Input.GetButton("AimTarget");
            _inputModel.IsInputWeaponChoise = Input.GetButton("WeaponWheel");

            CheckAxisTotal();
            CheckEvents();
        }

        private void CheckAxisTotal()
        {
            _inputModel.InputTotalAxisX = _inputModel.InputAxisX > 0 ? 1 : _inputModel.InputAxisX < 0 ? -1 : 0;
            _inputModel.InputTotalAxisY = _inputModel.InputAxisY > 0 ? 1 : _inputModel.InputAxisY < 0 ? -1 : 0;
        }

        private void CheckEvents()
        {
            foreach (var onButtonDownInput in _inputModel.inputOnButtonDown)
            {
                if (!onButtonDownInput.Key.Contains("NO_BUTTON") && Input.GetButtonDown(onButtonDownInput.Key))
                {
                    _eventManager.TriggerEvent(onButtonDownInput.Value);
                }                 
            }
        }

        #endregion
    }
}


