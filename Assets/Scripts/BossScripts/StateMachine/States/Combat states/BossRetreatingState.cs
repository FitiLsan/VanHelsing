using UnityEngine;


namespace BeastHunter
{
    public class BossRetreatingState : BossBaseState
    {
        #region Constants

        private const int FAKE_TREE_ID = 0;
        private const float ANIMATION_DELAY = 2f;

        #endregion


        #region Fields


        #endregion


        #region ClassLifeCycle

        public BossRetreatingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void Execute()
        {
            CheckNextMove();
        }

        public override void Initialise()
        {
            Debug.Log($"current State RETREATING initialise");
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;
            _bossData.SetNavMeshAgentSpeed(_bossModel.BossNavAgent, 0);
            _stateMachine._model.BossAnimator.Play("IdleState");
        }

        public override void OnAwake()
        {
            IsBattleState = true;
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
            if (_stateMachine.BossSkills.RetreatingStateSkillDictionary[FAKE_TREE_ID].IsSkillReady)
            {
                _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.RetreatingStateSkillDictionary, FAKE_TREE_ID);
            }
            else
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Defencing);
            }
        }
        #endregion

    }
}