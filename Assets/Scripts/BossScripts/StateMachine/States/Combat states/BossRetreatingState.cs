using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public class BossRetreatingState : BossBaseState
    {
        #region Constants

        private const float MINIMAL_TARGET_DISTANCE = -10f;
        private const float MAXIMAL_TARGET_DISTANCE = 10f;
        private const int FAKE_TREE_ID = 0;
        private const float ANIMATION_DELAY = 2f;

        #endregion


        #region Fields

        private LureProjectileData _currentTargetSmell;
        private bool _isTargetSet;
        private Vector3 _checkTargetPosition;

        #endregion


        #region ClassLifeCycle

        public BossRetreatingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void Execute()
        {
            SetTargetPoint();
            CheckisEscaped();
            CheckNextMove();
        }

        public override void Initialise()
        {
            Debug.Log($"current State RETREATING initialise");
            CanExit = false;
            CanBeOverriden = true;
            _isTargetSet = false;
            // IsBattleState = true;
          //  _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, 0);
            _stateMachine._model.BossAnimator.Play("MovingState");
        }

        public override void OnAwake()
        {
            // IsBattleState = true;
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
            //    ChoosingAttackSkill();
            }
        }

        //private void ChoosingAttackSkill()
        //{
        //    if (_stateMachine.BossSkills.RetreatingStateSkillDictionary[FAKE_TREE_ID].IsSkillReady)
        //    {
        //        _stateMachine.BossSkills.ForceUseSkill(_stateMachine.BossSkills.RetreatingStateSkillDictionary, FAKE_TREE_ID);
        //    }
        //    else
        //    {
        //        _stateMachine.SetCurrentStateOverride(BossStatesEnum.Defencing);
        //    }
        //}

        private bool CheckIsTargetDangerous()
        {

            _currentTargetSmell = _bossModel.BossCurrentTarget.GetComponent<ProjectileBehavior>().ProjectileData as LureProjectileData;
            if (_bossModel.CurrentStats.ItemReactions.ScaryItems.Find(x => x.LureSmellType.Equals(_currentTargetSmell.LureSmellType)))
            {
                return true;
            }
            return false;
        }

        private void SetTargetPoint()
        {
          CheckIsTargetDangerous();
            if (_isTargetSet)
            {
                return;
            }
            var smellingDistance = _currentTargetSmell.SmellingDistance;
            var residualDistance = smellingDistance - _bossData.GetTargetDistance(_bossModel.BossCurrentTarget.transform.position, _bossModel.CurrentPosition);
            var pointOutOfSmelling = _bossModel.BossCurrentTarget.transform.position +
                (_bossModel.CurrentPosition - _bossModel.BossCurrentTarget.transform.position).normalized *
                (residualDistance + residualDistance * 0.1f);
                _checkTargetPosition = Services.SharedInstance.PhysicsService.GetGroundedPosition(
                new Vector3(
                pointOutOfSmelling.x + Random.Range(MINIMAL_TARGET_DISTANCE, MAXIMAL_TARGET_DISTANCE),
                pointOutOfSmelling.y,
                pointOutOfSmelling.z + Random.Range(MINIMAL_TARGET_DISTANCE, MAXIMAL_TARGET_DISTANCE)), 30f);

            NavMeshPath path = new NavMeshPath();
            _bossModel.BossNavAgent.CalculatePath(_checkTargetPosition, path);
            if(path.status == NavMeshPathStatus.PathComplete)
            {
                _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _checkTargetPosition, _bossModel.BossSettings.RunSpeed);
                _isTargetSet = true;
            }
        }

        private void CheckisEscaped()
        {
            if (_isTargetSet &&  _bossData.CheckIsNearTarget(_bossModel.CurrentPosition, _checkTargetPosition, 2f))
            {
                _isTargetSet = false;
                _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, 0f);
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle);
            }
        }

        #endregion

    }
}