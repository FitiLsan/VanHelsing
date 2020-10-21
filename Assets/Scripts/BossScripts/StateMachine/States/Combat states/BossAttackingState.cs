﻿using UnityEngine;


namespace BeastHunter
{
    public class BossAttackingState : BossBaseState, IDealDamage
    {
        #region Constants

        private const float LOOK_TO_TARGET_SPEED = 1f;
        private const float PART_OF_NONE_ATTACK_TIME_LEFT = 0.15f;
        private const float PART_OF_NONE_ATTACK_TIME_RIGHT = 0.3f;
        private const float ANGLE_SPEED = 100f;
        private const float ANGLE_TARGET_RANGE_MIN = 10f;

        #endregion


        #region Fields

        private Vector3 _lookDirection;
        private Quaternion _toRotation;

        private bool _isCurrentAttackRight;

        private int _attackNumber;

        private float _currentAttackTime;

        #endregion


        #region ClassLifeCycle

        public BossAttackingState(BossStateMachine stateMachine) : base(stateMachine)
        {
        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _stateMachine._model.LeftWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
            _stateMachine._model.RightWeaponBehavior.OnFilterHandler += OnHitBoxFilter;
            _stateMachine._model.LeftWeaponBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
            _stateMachine._model.RightWeaponBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;
            
            _stateMachine._model.BossNavAgent.SetDestination(_stateMachine._model.BossTransform.position);
            _stateMachine._model.BossNavAgent.speed = 0f;

            _stateMachine._model.WeaponData.MakeSimpleAttack(out _attackNumber, _stateMachine._model.BossTransform);
            _currentAttackTime = _stateMachine._model.WeaponData.CurrentAttack.AttackTime;
            _stateMachine._model.BossAnimator.Play(_stateMachine._model.WeaponData.SimpleAttackAnimationPrefix + "Attack_" + _attackNumber, 0, 0f);

            if(_stateMachine._model.WeaponData.CurrentAttack.AttackType == HandsEnum.Left)
            {
                TimeRemaining enableWeapon = new TimeRemaining(() => _stateMachine._model.LeftWeaponBehavior.IsInteractable = true,
                    _currentAttackTime * PART_OF_NONE_ATTACK_TIME_LEFT);
                enableWeapon.AddTimeRemaining(_currentAttackTime * PART_OF_NONE_ATTACK_TIME_LEFT);
            }
            else
            {
                TimeRemaining enableWeapon = new TimeRemaining(() => _stateMachine._model.RightWeaponBehavior.IsInteractable = true,
                    _currentAttackTime * PART_OF_NONE_ATTACK_TIME_RIGHT);
                enableWeapon.AddTimeRemaining(_currentAttackTime * PART_OF_NONE_ATTACK_TIME_RIGHT);
            }          
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
            _stateMachine._model.LeftWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
            _stateMachine._model.RightWeaponBehavior.OnFilterHandler -= OnHitBoxFilter;
            _stateMachine._model.LeftWeaponBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
            _stateMachine._model.RightWeaponBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
        }

        private void CheckNextMove()
        {
            if(_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }
            if (_currentAttackTime <= 0)
            {
                DecideNextMove();
            }
        }

        private bool CheckDirection()
        {
            bool isNear = Quaternion.Angle(_stateMachine._model.BossTransform.rotation,
                _stateMachine._mainState.TargetRotation) <= BossMainState.ANGLE_TARGET_RANGE;

            if (!isNear)
            {
                CheckTargetDirection();
                TargetOnPlayer();
            }

            return isNear;
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

        private void DecideNextMove()
        {
            _stateMachine._model.LeftWeaponBehavior.IsInteractable = false;
            _stateMachine._model.RightWeaponBehavior.IsInteractable = false;

            if (!_stateMachine._model.IsDead && CheckDirection() && CheckDistance())
            {
                Initialise();
            }
        }

        private bool OnHitBoxFilter(Collider hitedObject)
        {         
            bool isEnemyColliderHit = hitedObject.CompareTag(TagManager.PLAYER);

            if (hitedObject.isTrigger || _stateMachine.CurrentState != _stateMachine.States[BossStatesEnum.Attacking])
            {
                isEnemyColliderHit = false;
            }

            return isEnemyColliderHit;
        }

        private void OnLeftHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void CheckTargetDirection()
        {
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


        private void TargetOnPlayer()
        {
            if (Quaternion.Angle(_stateMachine._model.BossTransform.rotation,
                _stateMachine._mainState.TargetRotation) > ANGLE_TARGET_RANGE_MIN)
            {
                _stateMachine._model.BossTransform.rotation = Quaternion.RotateTowards(_stateMachine.
                    _model.BossTransform.rotation, _stateMachine._mainState.TargetRotation, ANGLE_SPEED * Time.deltaTime);
            }
        }

        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            if (enemy != null && damage != null)
            {
                enemy.TakeDamageEvent(damage);
            }
        }

        #endregion

        #endregion
    }
}

