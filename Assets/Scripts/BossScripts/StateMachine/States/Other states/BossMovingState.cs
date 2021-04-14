using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class BossMovingState : BossBaseState
    {

        #region Fields
        private const float ANGLE_SPEED = 100f;
        private const float ANGLE_RANGE = 5f;
        private const float DISTANCE_TO_START_EATING = 2.3f;

        private Vector3 _target;
        private Quaternion TargetRotation;

        #endregion


        #region ClassLifeCycle

        public BossMovingState(BossStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        #endregion

        #region Methods

        public override void OnAwake()
        {
            CanBeOverriden = true;
            CanExit = true;
           
        }

        public override void Initialise()
        {
            _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, _bossData._bossSettings.WalkSpeed);
            _bossModel.BossNavAgent.stoppingDistance = DISTANCE_TO_START_EATING;
            _bossModel.BossAnimator.Play("MovingState");

            if (_bossModel.BossCurrentTarget != null)
            {
                _target = Services.SharedInstance.PhysicsService.GetGroundedPosition(_bossModel.BossCurrentTarget.transform.position, 30f);
            }
        }

        public override void Execute()
        {
            if (_bossModel.BossCurrentTarget != null && _target!= Vector3.zero && !CheckDistance())
            {
                MoveTo();
                RotateTo();
            }
            else
            {
                _bossData.SetNavMeshAgentSpeed(_bossModel, _bossModel.BossNavAgent, 0f);
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Idle) ;
            }
        }

        public override void OnExit()
        {
            _target = Vector3.zero;
        }

        public override void OnTearDown()
        {
        }


        private bool CheckDistance()
        {
            var isNear = _bossModel.BossData.CheckIsNearTarget(_bossModel.BossTransform.position, _target, DISTANCE_TO_START_EATING, ANGLE_RANGE);
            return isNear;
        }

        private void MoveTo()
        {
            _bossModel.BossData.NavMeshMoveTo(_bossModel.BossNavAgent, _target, _bossModel.BossData._bossSettings.WalkSpeed);
        }

        private void RotateTo()
        {
            _bossModel.BossTransform.rotation = _bossModel.BossData.RotateTo(_bossModel.BossTransform, _bossModel.BossCurrentTarget.transform , ANGLE_SPEED);
        }

        #endregion
    }
}