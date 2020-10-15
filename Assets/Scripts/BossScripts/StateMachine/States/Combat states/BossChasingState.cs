using UnityEngine;


namespace BeastHunter
{
    public class BossChasingState : BossBaseState
    {
        #region Constants

        private const float DISTANCE_TO_START_ATTACK = 1.5f;
        private const float TRIGGER_VIEW_INCREASE = 5f;

        #endregion


        #region Fields

        private Vector3 _target;

        #endregion


        #region ClassLifeCycle

        public BossChasingState(BossStateMachine stateMachine) : base(stateMachine)
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
            CanBeOverriden = true;    
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.RunSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_ATTACK;
            _stateMachine._model.BossSphereCollider.radius += TRIGGER_VIEW_INCREASE;
            _stateMachine._model.BossAnimator.Play("MovingState");
        }

        public override void Execute()
        {
            CheckTarget();
            CheckDistance();
        }

        public override void OnExit()
        {
            _stateMachine._model.BossSphereCollider.radius -= TRIGGER_VIEW_INCREASE;
        }

        public override void OnTearDown()
        {
        }

        private void CheckTarget()
        {
            _target = _stateMachine._context.CharacterModel.CharacterTransform.position;
            _stateMachine._model.BossNavAgent.SetDestination(_target);
        }

        private void CheckDistance()
        {
            if(Mathf.Sqrt((_stateMachine._model.BossTransform.position - _target)
                .sqrMagnitude) <= DISTANCE_TO_START_ATTACK)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Targeting);
            }
        }

        #endregion
    }
}

