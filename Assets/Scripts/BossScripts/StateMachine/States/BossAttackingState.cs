using UnityEngine;


namespace BeastHunter
{
    public class BossAttackingState : BossBaseState, IDealDamage
    {
        #region Constants

        private const float DISTANCE_TO_START_ATTACK = 2.5f;
        private const float LOOK_TO_TARGET_SPEED = 1.0f;
        private const float PART_OF_NONE_ATTACK_TIME_LEFT = 0.15f;
        private const float PART_OF_NONE_ATTACK_TIME_RIGHT = 0.3f;

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

            _stateMachine._model.WeaponData.MakeSimpleAttack(out _attackNumber);
            _currentAttackTime = _stateMachine._model.WeaponData.CurrentAttack.AttackTime;
            _stateMachine._model.BossAnimator.Play(_stateMachine._model.WeaponData.SimpleAttackAnimationPrefix + "Attack_" + _attackNumber, 0, 0f);

            if (_stateMachine._model.WeaponData.CurrentAttack.AttackType == HandsEnum.Left)
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
            CheckDirection();
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

        private void SetLeftWeaponInteractable()
        {
            _stateMachine._model.LeftWeaponBehavior.IsInteractable = true;
        }

        private void SetRightWeaponInteractable()
        {
            _stateMachine._model.RightWeaponBehavior.IsInteractable = true;
        }

        private void CheckNextMove()
        {
            if(_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;

                if(_currentAttackTime <= 0)
                {
                    DecideNextMove();
                }
            }
        }

        private void CheckDirection()
        {
            _stateMachine._model.BossTransform.LookAt(_stateMachine._context.CharacterModel.CharacterTransform);
        }

        private void DecideNextMove()
        {
            _stateMachine._model.LeftWeaponBehavior.IsInteractable = false;
            _stateMachine._model.RightWeaponBehavior.IsInteractable = false;

            if (!_stateMachine._model.IsDead)
            {
                if (Mathf.Sqrt((_stateMachine._model.BossTransform.position - _stateMachine.
                    _context.CharacterModel.CharacterTransform.position).sqrMagnitude) <= DISTANCE_TO_START_ATTACK)
                {
                    Initialise();
                }
                else
                {
                    _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
                }
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
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (enemy.transform.GetComponent<InteractableObjectBehavior>() != null && hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_stateMachine._model.WeaponData, _stateMachine._model.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.PlayerBehavior.Stats));
                hitBox.IsInteractable = false;
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

