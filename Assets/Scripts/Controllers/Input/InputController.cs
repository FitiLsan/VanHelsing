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
            _inputModel.InputAxisX = 0;
            _inputModel.InputAxisY = 0;
            _inputModel.InputTotalAxisX = 0;
            _inputModel.InputTotalAxisY = 0;
            _inputModel.IsInputMove = false;
            _inputModel.IsInputRun = false;

            _inputModel.inputOnButtonDown.Add("NO_BUTTON_1", InputEventTypes.MoveStart);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_2", InputEventTypes.MoveStop);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_3", InputEventTypes.RunStart);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_4", InputEventTypes.RunStop);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_5", InputEventTypes.WeaponWheelOpen);
            _inputModel.inputOnButtonDown.Add("NO_BUTTON_6", InputEventTypes.WeaponWheelClose);
            _inputModel.inputOnButtonDown.Add("Jump", InputEventTypes.Jump);
            _inputModel.inputOnButtonDown.Add("Battle Exit", InputEventTypes.BattleExit);
            _inputModel.inputOnButtonDown.Add("Dodge", InputEventTypes.Dodge);
            _inputModel.inputOnButtonDown.Add("Target lock", InputEventTypes.TargetLock);
            _inputModel.inputOnButtonDown.Add("AttackLeft", InputEventTypes.AttackLeft);
            _inputModel.inputOnButtonDown.Add("AttackRight", InputEventTypes.AttackRight);
            _inputModel.inputOnButtonDown.Add("Use", InputEventTypes.Use);
            _inputModel.inputOnButtonDown.Add("Dance", InputEventTypes.Dance);
            _inputModel.inputOnButtonDown.Add("Cancel", InputEventTypes.Cancel);
            _inputModel.inputOnButtonDown.Add("NumberOne", InputEventTypes.NumberOne);
            _inputModel.inputOnButtonDown.Add("NumberTwo", InputEventTypes.NumberTwo);
            _inputModel.inputOnButtonDown.Add("NumberThree", InputEventTypes.NumberThree);
            _inputModel.inputOnButtonDown.Add("NumberFour", InputEventTypes.NumberFour);
            _inputModel.inputOnButtonDown.Add("Inventory", InputEventTypes.Inventory);
            _inputModel.inputOnButtonDown.Add("QuestJournal", InputEventTypes.QuestJournal);
            _inputModel.inputOnButtonDown.Add("PlaceTrap1", InputEventTypes.PlaceTrap1);
            _inputModel.inputOnButtonDown.Add("PlaceTrap2", InputEventTypes.PlaceTrap2);
            _inputModel.inputOnButtonDown.Add("Sneak", InputEventTypes.Sneak);
            _inputModel.inputOnButtonDown.Add("TimeSkip", InputEventTypes.TimeSkipMenu);
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


