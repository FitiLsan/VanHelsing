using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class InputModel
    {
        #region Fields

        public Dictionary<string, InputEventTypes> inputOnButtonDown;

        public float InputAxisX;
        public float InputAxisY;
        public float InputTotalAxisX;
        public float InputTotalAxisY;

        private bool _isInputMove;
        private bool _isInputRun;

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
                if(_isInputMove != value)
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
                if(_isInputRun != value)
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

        #endregion


        #region ClassLifeCycle

        public InputModel()
        {
            inputOnButtonDown = new Dictionary<string, InputEventTypes>();
        }

        #endregion
    }
}

