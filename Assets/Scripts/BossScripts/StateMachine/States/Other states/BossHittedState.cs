namespace BeastHunter
{
    public class BossHittedState : BossBaseState
    {
        #region Constants

        private const float EXIT_TIME = 0.8f;

        #endregion


        #region Fields

        private float _bossAcceleration;

        #endregion


        #region ClassLifeCycle

        public BossHittedState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = false;
            _stateMachine._model.BossNavAgent.enabled = false;
            _stateMachine._model.BossAnimator.Play("HittedState");
            _stateMachine._model.BossAnimator.SetFloat("HitNumber", UnityEngine.Random.Range(0, 5));
            TimeRemaining stopStun = new TimeRemaining(StopStun, EXIT_TIME);
            stopStun.AddTimeRemaining(EXIT_TIME);
        }

        public override void Execute()
        {
        }

        public override void OnExit()
        {
            _stateMachine._model.BossNavAgent.enabled = true;
        }

        public override void OnTearDown()
        {
        }

        private void StopStun()
        {
            CanBeOverriden = true;
            _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
        }

        #endregion
    }
}

