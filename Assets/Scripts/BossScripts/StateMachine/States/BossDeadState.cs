using UnityEngine;


namespace BeastHunter
{
    public class BossDeadState : BossBaseState
    {
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
            CanExit = false;
            CanBeOverriden = false;
            _stateMachine._model.BossNavAgent.enabled = false;
            _stateMachine._model.IsDead = true;
            _stateMachine._model.BossAnimator.Play("DeadState", 0, 0f);
            _stateMachine._model.BossCapsuleCollider.center = Vector3.zero;
            _stateMachine._model.BossCapsuleCollider.height = 1f;
            _stateMachine._model.FirstWeakPointBehavior.gameObject.SetActive(false);
            _stateMachine._model.SecondWeakPointBehavior.gameObject.SetActive(false);
            _stateMachine._model.ThirdWeakPointBehavior.gameObject.SetActive(false);
            _stateMachine._model.BossTransform.tag = TagManager.UNTAGGED;
            GlobalEventsModel.OnBossDie?.Invoke();
            _stateMachine._context.CharacterModel.EnemiesInTrigger.Remove(_stateMachine._model.BossCapsuleCollider);
        }

        public override void Execute()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        #endregion
    }
}
