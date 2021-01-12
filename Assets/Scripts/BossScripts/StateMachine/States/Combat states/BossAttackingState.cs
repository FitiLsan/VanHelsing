using RootMotion.Dynamics;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class BossAttackingState : BossBaseState, IDealDamage
    {
        #region Constants
        //private const float ANGLE_SPEED = 150f;
        //private const float ANGLE_TARGET_RANGE_MIN = 20f;
        //private const float DISTANCE_TO_START_ATTACK = 4f;
        private const float DELAY_HAND_TRIGGER = 0.2f;
        private const float ANIMATION_DELAY = 0.2f;

        //private const int DEFAULT_ATTACK_ID = 0;

        #endregion


        #region Fields

        private Vector3 _lookDirection;
        private Quaternion _toRotation;
        

        private int _skillId;
        private Dictionary<int,int> _readySkillDictionary = new Dictionary<int, int>();

        #endregion


        #region ClassLifeCycle

        public BossAttackingState(BossStateMachine stateMachine) : base(stateMachine)
        {

        }

        #endregion


        #region Methods

        public override void OnAwake()
        {
            _bossModel.LeftHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _bossModel.RightHandBehavior.OnFilterHandler += OnHitBoxFilter;
            _bossModel.LeftHandBehavior.OnTriggerEnterHandler += OnLeftHitBoxHit;
            _bossModel.RightHandBehavior.OnTriggerEnterHandler += OnRightHitBoxHit;
            _bossModel.InteractionSystem.OnInteractionPickUp += OnPickUp;
            ThrowAttackSkill.HandDrop += OnDrop;
        }

        public override void Initialise()
        {
            CanExit = false;
            CanBeOverriden = true;
            IsBattleState = true;
            base.CurrentAttackTime = 0f;
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);

            for (var i = 0; i < _stateMachine.BossSkills.AttackStateSkillDictionary.Count; i++)
            {
                _stateMachine.BossSkills.AttackStateSkillDictionary[i].StartCooldown(_stateMachine.BossSkills.AttackStateSkillDictionary[i].SkillId, _stateMachine.BossSkills.AttackStateSkillDictionary[i].SkillCooldown);
            }
          //  ChoosingAttackSkill();
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
            _bossModel.LeftHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _bossModel.RightHandBehavior.OnFilterHandler -= OnHitBoxFilter;
            _bossModel.LeftHandBehavior.OnTriggerEnterHandler -= OnLeftHitBoxHit;
            _bossModel.RightHandBehavior.OnTriggerEnterHandler -= OnRightHitBoxHit;
            _bossModel.InteractionSystem.OnInteractionPickUp -= OnPickUp;
            ThrowAttackSkill.HandDrop -= OnDrop;
        }

        private void ChoosingAttackSkill(bool isDefault = false)
        {
            _readySkillDictionary.Clear();
            var j = 0;


            for (var i = 0; i < _stateMachine.BossSkills.AttackStateSkillDictionary.Count; i++)
            {
                if (_stateMachine.BossSkills.AttackStateSkillDictionary[i].IsSkillReady)
                {
                    if (CheckDistance(_stateMachine.BossSkills.AttackStateSkillDictionary[i].SkillRangeMin, _stateMachine.BossSkills.AttackStateSkillDictionary[i].SkillRangeMax))
                    {
                        _readySkillDictionary.Add(j, i);
                        j++;
                    }
                }
            }

            if(_readySkillDictionary.Count==0 & _bossData.GetTargetDistance(_bossModel.BossTransform.position, _bossModel.BossCurrentTarget.transform.position)>=DISTANCE_TO_START_ATTACK)
            {
                _stateMachine.SetCurrentStateOverride(BossStatesEnum.Chasing);
                return;
            }

            if (!isDefault & _readySkillDictionary.Count!=0)
            {
                var readyId = UnityEngine.Random.Range(0, _readySkillDictionary.Count);
                _skillId = _readySkillDictionary[readyId];
            }
            else
            {
                _skillId = DEFAULT_ATTACK_ID;
            }

            // _stateMachine.BossSkills.AttackStateSkillDictionary[_skillId].UseSkill(_skillId);
            CurrentSkill = _stateMachine.BossSkills.AttackStateSkillDictionary[_skillId];
            CurrentSkill.UseSkill(_skillId);
            isAnySkillUsed = true;
        }

        private void CheckNextMove()
        {
            if (isAnimationPlay)
            {
                base.CurrentAttackTime = _bossModel.BossAnimator.GetCurrentAnimatorStateInfo(0).length + ANIMATION_DELAY;
                isAnimationPlay = false;
            }

            if (base.CurrentAttackTime > 0)
            {
                base.CurrentAttackTime -= Time.deltaTime;
                
            }
            if (base.CurrentAttackTime <= 0)
            {
                DecideNextMove();
            }
        }

        private void DecideNextMove()
        {
            SetNavMeshAgent(_bossModel.BossTransform.position, 0);
            _bossModel.LeftHandBehavior.IsInteractable = false;
            _bossModel.RightHandBehavior.IsInteractable = false;
            _bossModel.LeftHandCollider.enabled = false;
            _bossModel.RightHandCollider.enabled = false;

            if (!_bossModel.IsDead && CheckDirection())
            {
                ChoosingAttackSkill();
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
                    CountDamage(_bossModel.WeaponData, _bossModel.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.CharacterStats));
                hitBox.IsInteractable = false;
                _bossModel.LeftHandCollider.enabled = false;
                
            }
        }

        private void OnRightHitBoxHit(ITrigger hitBox, Collider enemy)
        {
            if (hitBox.IsInteractable)
            {
                DealDamage(_stateMachine._context.CharacterModel.PlayerBehavior, Services.SharedInstance.AttackService.
                    CountDamage(_bossModel.WeaponData, _bossModel.BossStats.MainStats, _stateMachine.
                        _context.CharacterModel.CharacterStats));
                hitBox.IsInteractable = false;
                _bossModel.RightHandCollider.enabled = false;
            }
        }

        //private void CheckTargetDirection()
        //{
        //    Vector3 heading = _bossModel.BossCurrentTarget.transform.position -
        //        _bossModel.BossTransform.position;

        //    int directionNumber = _bossData.AngleDirection(
        //        _bossModel.BossTransform.forward, heading, _bossModel.BossTransform.up);

        //    switch (directionNumber)
        //    {
        //        case -1:
        //            _bossModel.BossAnimator.Play("TurningLeftState", 0, 0f);
        //            break;
        //        case 0:
        //            _bossModel.BossAnimator.Play("IdleState", 0, 0f);
        //            break;
        //        case 1:
        //            _bossModel.BossAnimator.Play("TurningRightState", 0, 0f);
        //            break;
        //        default:
        //            _bossModel.BossAnimator.Play("IdleState", 0, 0f);
        //            break;
        //    }
        //}

        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            if (enemy != null && damage != null)
            {
                enemy.TakeDamageEvent(damage);
            }
        }

        #endregion

        private void OnPickUp(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            Debug.Log("PickUP");
            _bossModel.IsPickUped = true;
            _stateMachine._context.CharacterModel.CharacterRigitbody.isKinematic = true;
            _stateMachine._context.CharacterModel.PuppetMaster.mode = PuppetMaster.Mode.Disabled;

            _bossModel.CurrentHand = effectorType;
            if (effectorType == FullBodyBipedEffector.LeftHand)
            {
               _bossModel.BossAnimator.SetFloat("IdleState", 11);
                _bossModel.BossAnimator.Play("Catch_Blend_Idle", 0, 0);
            }
            if (effectorType == FullBodyBipedEffector.RightHand)
            {
                _bossModel.BossAnimator.SetFloat("IdleState", 1);
                _bossModel.BossAnimator.Play("Catch_Blend_Idle", 0, 0);
            }
        }
        
        private void OnDrop()
        {
            Debug.Log("DROP");
            _stateMachine._context.CharacterModel.CharacterTransform.rotation = Quaternion.Euler(0, 0, 0);
            _stateMachine._context.CharacterModel.CharacterRigitbody.isKinematic = true;
           // _stateMachine._context.CharacterModel.CharacterAnimator.enabled = false;
            _stateMachine._context.CharacterModel.PuppetMaster.mode = PuppetMaster.Mode.Disabled;
            _stateMachine._context.CharacterModel.PuppetMaster.state = PuppetMaster.State.Frozen;
            TimeRemaining timeRemaining = new TimeRemaining(() => _stateMachine._context.CharacterModel.PuppetMaster.state = PuppetMaster.State.Alive, 1f);
            timeRemaining.AddTimeRemaining(1f);

            if (_bossModel.CurrentHand == FullBodyBipedEffector.RightHand)
            {
                _bossModel.BossAnimator.Play("IdleState", 0, 0);
            }
            if (_bossModel.CurrentHand == FullBodyBipedEffector.LeftHand)
            {
                _bossModel.BossAnimator.Play("IdleState", 0, 0);
            }

            _bossModel.CatchTarget.transform.parent = _bossModel.targetParent.transform;

            //_bossModel.BossAnimator.SetFloat("IdleState", 6);
            //_bossModel.BossAnimator.Play("Catch_Blend_Idle", 0, 0);
            _bossModel.IsPickUped = false;
        }

        #endregion
    }
}

