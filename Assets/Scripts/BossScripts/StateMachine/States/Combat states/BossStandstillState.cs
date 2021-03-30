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
            if (CurrentEffectType == BuffEffectType.Fire)
                FireReaction();
        }

        public override void Execute()
        {
        }

        public override void OnExit()
        {
            CurrentEffectType = BuffEffectType.None;
        }

        public override void OnTearDown()
        {
        }

        protected override void FireReaction()
        {
            var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(_bossModel.BossTransform.position, 500f, "Water");

            if (list.Count != 0)
            {
                _bossModel.BossAnimator.Play("MovingState", 0, 0);
                _bossData.SetNavMeshAgent(_bossModel.BossNavAgent, list[0].transform.position, _bossModel.BossSettings.RunSpeed);

            }
            base.FireReaction();
        }
        #endregion
    }
}
