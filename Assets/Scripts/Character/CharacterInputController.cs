using UnityEngine;
using UnityEngine.Events;


namespace BeastHunter
{
    public sealed class CharacterInputController
    {
        #region Properties

        public float InputAxisX { get; private set; }
        public float InputAxisY { get; private set; }

        public bool InputJump { get; private set; }
        public bool InputRun { get; private set; }
        public bool InputDodge { get; private set; }
        public bool InputBattleExit { get; private set; }
        public bool InputTargetLock { get; private set; }
        public bool InputAttack { get; private set; }

        public UnityEvent OnJump { get; private set; }
        public UnityEvent OnDodge { get; private set; }
        public UnityEvent OnAttack { get; private set; }
        public UnityEvent OnTargetLock { get; private set; }
        public UnityEvent OnBattleExit { get; private set; }

        #endregion


        #region Methods

        public void Initialize()
        {
            InputAxisX = 0;
            InputAxisY = 0;

            InputJump = false;
            InputRun = false;
            InputDodge = false;
            InputBattleExit = false;
            InputTargetLock = false;
            InputAttack = false;

            if(OnJump == null)
            {
                OnJump = new UnityEvent();
            }

            if(OnDodge == null)
            {
                OnDodge = new UnityEvent();
            }

            if(OnAttack == null)
            {
                OnAttack = new UnityEvent();
            }

            if(OnTargetLock == null)
            {
                OnTargetLock = new UnityEvent();
            }

            if(OnBattleExit == null)
            {
                OnBattleExit = new UnityEvent();
            }
        }

        public void GetInput()
        {
            InputAxisX = Input.GetAxis("Horizontal");
            InputAxisY = Input.GetAxis("Vertical");
            InputRun = Input.GetButton("Sprint");
            InputJump = Input.GetButtonDown("Jump");
            InputBattleExit = Input.GetButtonDown("Battle Exit");
            InputDodge = Input.GetButtonDown("Dodge");
            InputTargetLock = Input.GetButtonDown("Target lock");
            InputAttack = Input.GetButtonDown("Fire");

            CheckEvents();
        }

        private void CheckEvents()
        {
            if (InputJump)
            {
                if (OnJump != null)
                {
                    OnJump.Invoke();
                }
            }

            if (InputDodge)
            {
                if(OnDodge != null)
                {
                    OnDodge.Invoke();
                }
            }

            if (InputAttack)
            {
                if (OnAttack != null)
                {
                    OnAttack.Invoke();
                }
            }

            if (InputTargetLock)
            {
                if(OnTargetLock != null)
                {
                    OnTargetLock.Invoke();
                }
            }

            if (InputBattleExit)
            {
                if(OnBattleExit != null)
                {
                    OnBattleExit.Invoke();
                }
            }
        }

        #endregion
    }
}


