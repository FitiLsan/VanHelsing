using UnityEngine;


namespace BeastHunter
{
    public class BossChasingState : BossBaseState
    {
        #region Constants

        private const float DISTANCE_TO_START_ATTACK = 4f;
        private const float TRIGGER_VIEW_INCREASE = 50f;
        private const float FORCE_ATTACK_TIME_MIN = 3f;
        private const float FORCE_ATTACK_TIME_MAX = 10f;
        private const float VINE_FISHING_DISTANCE = 10f;
        private const int VINE_FISHING_ID = 0;
        private const float ANIMATION_DELAY = 0.2f;

        #endregion


        #region Fields

        private Vector3 _target;
        private float _forceAttackTime;
        private float currentDistance;

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
            Debug.Log($"current State CHASING initialise");
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.RunSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_ATTACK;
            _stateMachine._model.BossAnimator.Play("MovingState");
            _forceAttackTime = Random.Range(FORCE_ATTACK_TIME_MIN, FORCE_ATTACK_TIME_MAX);
            StartCoolDownSkills(_bossSkills.ChasingStateSkillDictionary);
        }

        public override void Execute()
        {
            CheckTarget();
            CheckDistance();
            CheckNextMove();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CheckTarget()
        {
            _target = _stateMachine._model.BossCurrentTarget.transform.position;
            _stateMachine._model.BossNavAgent.SetDestination(_target);
        }

        private void CheckDistance()
        {
            if (_bossData.CheckIsNearTarget(_stateMachine._model.BossTransform.position, _target, DISTANCE_TO_START_ATTACK, out currentDistance))
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        private void CheckNextMove()
        {
            if (isAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                isAnimationPlay = false;
            }

            if (base.CurrentAttackTime > 0)
            {
                base.CurrentAttackTime -= Time.deltaTime;

            }
            if (base.CurrentAttackTime <= 0)
            {
                _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.RunSpeed;
                _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_ATTACK;
                _stateMachine._model.BossAnimator.Play("MovingState");
                CheckExtraAttack();
            }
        }

        private void CheckExtraAttack()
        {

            _forceAttackTime -= Time.deltaTime;
            if (_forceAttackTime <= 0)
            {
                if (currentDistance >= _stateMachine.BossSkills.ChasingStateSkillDictionary[VINE_FISHING_ID].SkillRangeMax && _stateMachine.BossSkills.ChasingStateSkillDictionary[VINE_FISHING_ID].IsSkillReady)
                {
                    _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.ChasingStateSkillDictionary, VINE_FISHING_ID);
                    _stateMachine.BossSkills.ChasingStateSkillDictionary[VINE_FISHING_ID].StartCooldown(_stateMachine.BossSkills.ChasingStateSkillDictionary[VINE_FISHING_ID].SkillId, _stateMachine.BossSkills.ChasingStateSkillDictionary[VINE_FISHING_ID].SkillCooldown);
                    _forceAttackTime = Random.Range(FORCE_ATTACK_TIME_MIN, FORCE_ATTACK_TIME_MAX);
                    return;
                }
                _forceAttackTime = Random.Range(FORCE_ATTACK_TIME_MIN, FORCE_ATTACK_TIME_MAX);
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        #endregion
    }
}

