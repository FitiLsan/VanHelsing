using UnityEngine;


namespace BeastHunter
{
    public class BossChasingState : BossBaseState
    {
        #region Constants

        private const float DISTANCE_TO_START_ATTACK = 4f;
        private const float TRIGGER_VIEW_INCREASE = 5f;
        private const float FORCE_ATTACK_TIME_MIN = 3f;
        private const float FORCE_ATTACK_TIME_MAX = 10f;

        #endregion


        #region Fields

        private Vector3 _target;
        private float _forceAttackTime;

        #endregion


        #region ClassLifeCycle

        public BossChasingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            IsBattleState = true;
        }

        public override void Initialise()
        {
            Debug.Log($"current State CHASING initialise");
            CanExit = false;
            CanBeOverriden = true;    
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.RunSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_ATTACK;
            _stateMachine._model.BossSphereCollider.radius += TRIGGER_VIEW_INCREASE;
            _stateMachine._model.BossAnimator.Play("MovingState");
            _forceAttackTime = Random.Range(FORCE_ATTACK_TIME_MIN, FORCE_ATTACK_TIME_MAX);
        }

        public override void Execute()
        {
            CheckTarget();
            CheckDistance();
            CheckExtraAttack();
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
            _target = _stateMachine._model.BossCurrentTarget.transform.position;//_context.CharacterModel.CharacterTransform.position;
            _stateMachine._model.BossNavAgent.SetDestination(_target);
        }

        private void CheckDistance()
        {
            if(_bossData.CheckIsNearTarget(_stateMachine._model.BossTransform.position, _target, DISTANCE_TO_START_ATTACK))
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        private void CheckExtraAttack()
        {
            _forceAttackTime -= Time.deltaTime;
            if (_forceAttackTime <= 0)
            {
                _forceAttackTime = Random.Range(FORCE_ATTACK_TIME_MIN, FORCE_ATTACK_TIME_MAX);
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        #endregion
    }
}

