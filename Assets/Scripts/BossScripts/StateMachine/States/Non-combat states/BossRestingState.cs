using UnityEngine;

namespace BeastHunter
{
    public class BossRestingState : BossBaseState
    {
        #region Constants

        private const float RESTING_TIME = 10f;
        private const float DISTANCE_TO_START_RESTING = 1.5f;

        #endregion


        #region Fields

        private Vector3 _target;
        private float _restingCountTime = RESTING_TIME;
        private bool _canRest;

        #endregion


        #region ClassLifeCycle

        public BossRestingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            CanExit = true;
            CanBeOverriden = true;
            IsBattleState = false;
        }

        public override void Initialise()
        {
            _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, _bossData._bossSettings.WalkSpeed);
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_RESTING;
            _stateMachine._model.BossAnimator.Play("MovingState");
        }

        public override void Execute()
        {
            CheckTarget();
            CheckDistance();
            Resting();
        }

        public override void OnExit()
        {
            
        }

        public override void OnTearDown()
        {

        }

        private void MoveToLair()
        {
            if (_stateMachine._model.Lair != null)
            {
                _stateMachine._model.BossCurrentTarget = _stateMachine._model.Lair;
                _stateMachine.SetCurrentState(BossStatesEnum.Moving);
            }
            else
            {
                //find nearest tree and go there
            }
        }

        private void CheckTarget()
        {
            if (_stateMachine._model.Lair != null)
            {
                _target = _stateMachine._model.Lair.transform.position;
            }
            else
            {
                _stateMachine.SetCurrentState(BossStatesEnum.Patroling);
            }
        }

        private void CheckDistance()
        {
            if (_bossModel.BossData.CheckIsNearTarget(_bossModel.BossTransform.position, _target, DISTANCE_TO_START_RESTING) 
                & _bossModel.CurrentStats.BaseStats.CurrentStaminaPoints >= _bossModel.CurrentStats.BaseStats.MaximalStaminaPoints)
            {
                _canRest = true;
            }
            else
            {
                _canRest = false;
                MoveToLair();
            }
        }

        private void Resting()
        {
            if (_canRest)
            {
                Debug.Log("Resting");
                _bossModel.BossAnimator.Play("RestingState",0,0);
                _restingCountTime -= Time.deltaTime;
                if (_restingCountTime <= 0)
                {
                    _canRest = false;
                    _restingCountTime = RESTING_TIME;
                    _bossModel.CurrentStats.BaseStats.CurrentStaminaPoints = 0f;
                    _bossModel.BossCurrentTarget = null;
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle);
                }
            }
        }
        #endregion

    }
}