namespace BeastHunter
{
    public sealed class InputModel
    {
        #region Fields

        public MainInput MainInput;

        public float MouseInputX;
        public float MouseInputY;
        public float InputAxisX;
        public float InputAxisY;
        public float InputTotalAxisX;
        public float InputTotalAxisY;

        private bool _isInputMove;
        private bool _isInputRun;
        private bool _isInputAttack;
        private bool _isInputAim;
        private bool _isInputWeaponChoise;

        #endregion


        #region Properties

        public bool IsInputMove
        {
            get
            {
                return _isInputMove;
            }
            set
            {
                if (_isInputMove != value)
                {
                    _isInputMove = value;

                    if (_isInputMove)
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.MoveStart);
                    }
                    else
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.MoveStop);
                    }
                }
            }
        }

        public bool IsInputRun
        {
            get
            {
                return _isInputRun;
            }
            set
            {
                if (_isInputRun != value)
                {
                    _isInputRun = value;

                    if (_isInputRun)
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.RunStart);
                    }
                    else
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.RunStop);
                    }
                }
            }
        }

        public bool IsInputAttack
        {
            get
            {
                return _isInputAttack;
            }
            set
            {
                if (_isInputAttack != value)
                {
                    _isInputAttack = value;

                    if (_isInputAttack)
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.AttackStart);
                    }
                    else
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.AttackEnd);
                    }
                }
            }
        }

        public bool IsInputAim
        {
            get
            {
                return _isInputAim;
            }
            set
            {
                if (_isInputAim != value)
                {
                    _isInputAim = value;

                    if (_isInputAim)
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.AimStart);
                    }
                    else
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.AimEnd);
                    }
                }
            }
        }

        public bool IsInputWeaponChoise
        {
            get
            {
                return _isInputWeaponChoise;
            }
            set
            {
                if (_isInputWeaponChoise != value)
                {
                    _isInputWeaponChoise = value;

                    if (_isInputWeaponChoise)
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.WeaponWheelOpen);
                    }
                    else
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(InputEventTypes.WeaponWheelClose);
                    }
                }
            }
        }

        #endregion


        #region ClassLifeCycle

        public InputModel()
        {
            MainInput = new MainInput();
        }

        #endregion
    }
}

