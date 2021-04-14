using UnityEngine;


namespace BeastHunter
{
    public class BossStandstillState : BossBaseState
    {
        #region Constants

        private const float ANIMATION_DELAY = 0.2f;

        #endregion

        #region ClassLifeCycle

        public BossStandstillState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
        }

        public override void Initialise()
        {
            Debug.Log($"current State STANDSTILL initialise");
            CanExit = false;
            CanBeOverriden = false;
            IsBattleState = true;
            ChooseReaction();
        }

        public override void Execute()
        {
            if (CurrentEffectType == BuffEffectType.None)
            {
                _stateMachine.SetCurrentStateAnyway(_stateMachine.LastStateType);
            }
        }

        public override void OnExit()
        {
            CurrentEffectType = BuffEffectType.None;
        }

        public override void OnTearDown()
        {
        }

        private void ChooseReaction()
        {
            switch (CurrentEffectType)
            {
                case BuffEffectType.Fire:
                    FireReaction();
                    break;
                case BuffEffectType.Water:
                    WaterReaction();
                    break;
                default:
                    break;
            }
        }
        protected override void FireReaction()
        {
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.BossTransform.position, 500f, "Water");

            if (list.Count != 0)
            {
                _bossModel.BossAnimator.Play("MovingState", 0, 0);
                _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, list[0].transform.position, _bossModel.BossSettings.RunSpeed);

            }
            else
            {
                _bossModel.CheckIsRage(true);
                _stateMachine.SetCurrentStateAnyway(_stateMachine.LastStateType);
            }
            base.FireReaction();
        }

        protected override void WaterReaction()
        {
            base.WaterReaction();
        }
        #endregion
    }
}
