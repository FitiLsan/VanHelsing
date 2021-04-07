using System;


namespace BeastHunter
{
    public sealed class InputModel
    {
        #region Fields

        public Action OnMove;
        public Action OnStop;
        public Action OnRunStart;
        public Action OnRunStop;
        public Action OnAttack;
        public Action OnUse;
        public Action<bool> OnWeaponWheel;
        public Action OnRemoveWeapon;
        public Action OnAim;
        public Action OnSneakSlide;
        public Action OnJump;
        public Action OnPressNumberOne;
        public Action OnPressNumberTwo;
        public Action OnPressNumberThree;
        public Action OnPressNumberFour;
        public Action OnBestiary;
        public Action OnButtonsInfo;
        public Action OnPressEnter;
        public Action OnPressCancel;

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
                        OnMove?.Invoke();
                    }
                    else
                    {
                        OnStop?.Invoke();
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
                        OnRunStart?.Invoke();
                    }
                    else
                    {
                        OnRunStop?.Invoke();
                    }
                }
            }
        }

        #endregion
    }
}

