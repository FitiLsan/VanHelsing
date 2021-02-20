using UnityEngine;


namespace BeastHunter
{
    public class BossDeadState : BossBaseState
    {
        #region Constants

        private const int RESURRECTION_SKILL_ID = 0;
        private const float ANIMATION_DELAY = 0.2f;
        private const float RESURRECTION_SKILL_DELAY = 10f;

        private float _resurrectionDelay = RESURRECTION_SKILL_DELAY;
        #endregion

        #region ClassLifeCycle

        public BossDeadState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
        }

        public override void Initialise()
        {
            Debug.Log($"current State DEAD initialise");
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;
            _bossModel.BossNavAgent.enabled = false;
            _bossModel.CurrentStats.BaseStats.IsDead = true;
            _bossModel.BossAnimator.Play("DeadState", 0, 0f);
            //_stateMachine._model.BossCapsuleCollider.center = Vector3.zero;
            //_stateMachine._model.BossCapsuleCollider.height = 1f;
            //_stateMachine._model.FirstWeakPointBehavior.gameObject.SetActive(false);
            //_stateMachine._model.SecondWeakPointBehavior.gameObject.SetActive(false);
            //_stateMachine._model.ThirdWeakPointBehavior.gameObject.SetActive(false);
            //_stateMachine._model.BossBehavior.SetType(InteractableObjectType.None);
            //_stateMachine._context.CharacterModel.EnemiesInTrigger.Remove(_stateMachine._model.BossCapsuleCollider);
            _resurrectionDelay = RESURRECTION_SKILL_DELAY;
        }

        public override void Execute()
        {
            CheckNextMove();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void CheckNextMove()
        {
            if (IsAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                IsAnimationPlay = false;
            }

            if (base.CurrentAttackTime > 0)
            {
                base.CurrentAttackTime -= Time.deltaTime;

            }
            if (base.CurrentAttackTime <= 0)
            {
                ChoosingAttackSkill();
            }
        }

        private void ChoosingAttackSkill()
        {
            _resurrectionDelay -= Time.deltaTime;
            {
                if (_resurrectionDelay <= 0 && _stateMachine.BossSkills.DeadStateSkillDictionary.ContainsKey(RESURRECTION_SKILL_ID))
                {
                    if (_stateMachine.BossSkills.DeadStateSkillDictionary[RESURRECTION_SKILL_ID].IsSkillReady)
                    {
                        _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.DeadStateSkillDictionary, RESURRECTION_SKILL_ID);
                        return;
                    }
                    if (_bossModel.CurrentStats.BaseStats.IsDead == false)
                    {
                        _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
                    }
                }
            }
        }
        #endregion
    }
}
