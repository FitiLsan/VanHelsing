using UnityEngine;


namespace BeastHunter
{
    public class BossTargetingState : BossBaseState
    {
        #region 

        private const float ANGLE_SPEED = 100f;
        private const float ANGLE_TARGET_RANGE_MIN = 10f;

        #endregion

        #region ClassLifeCycle

        public BossTargetingState(BossStateMachine stateMachine) : base(stateMachine)
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

            Vector3 heading = _stateMachine._context.CharacterModel.CharacterTransform.position - 
                _stateMachine._model.BossTransform.position;
            int directionNumber = _stateMachine._mainState.AngleDirection(
                _stateMachine._model.BossTransform.forward, heading, _stateMachine._model.BossTransform.up);

            switch (directionNumber)
            {
                case -1:
                    _stateMachine._model.BossAnimator.Play("TurningLeftState", 0, 0f);
                    break;
                case 0:
                    _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
                    break;
                case 1:
                    _stateMachine._model.BossAnimator.Play("TurningRightState", 0, 0f);
                    break;
                default:
                    _stateMachine._model.BossAnimator.Play("IdleState", 0, 0f);
                    break;
            }
        }

        public override void Execute()
        {
            TargetOnPlayer();
        }

        public override void OnExit()
        {
        }

        public override void OnTearDown()
        {
        }

        private void TargetOnPlayer()
        {
            if (Quaternion.Angle(_stateMachine._model.BossTransform.rotation, 
                _stateMachine._mainState.TargetRotation) > ANGLE_TARGET_RANGE_MIN)
            {
                _stateMachine._model.BossTransform.rotation = Quaternion.RotateTowards(_stateMachine.
                    _model.BossTransform.rotation, _stateMachine._mainState.TargetRotation, ANGLE_SPEED * Time.deltaTime);
            }
            else if(CheckDistance())
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        private bool CheckDistance()
        {
            bool isNear = Mathf.Sqrt((_stateMachine._model.BossTransform.position - _stateMachine.
                _context.CharacterModel.CharacterTransform.position).sqrMagnitude) <= BossMainState.DISTANCE_TO_START_ATTACK;

            if (!isNear)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
            }

            return isNear;
        }

        #endregion
    }
}

