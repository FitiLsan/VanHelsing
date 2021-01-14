using Extensions;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewHellHound", menuName = "CreateData/HellHound", order = 2)]
    public sealed class HellHoundData : EnemyData, IDealDamage
    {
        #region PrivateData

        public enum BehaviourState
        {
            None,
            Roaming,
            Idling,
            Chasing,
            JumpingBack,
            BattleCircling,
            Escaping,
            Resting,
            Searching,
            JumpingAttack,
        }

        #endregion


        #region Debug messages

        private Action _noneStateMsg;
        private Action _idlingStateMsg;
        private Action _roamingStateMsg;
        private Action _restingStateMsg;
        private Action _chasingStateMsg;
        private Action _backJumpingStateMsg;
        private Action _battleCirclingStateMsg;
        private Action _searchingStateMsg;
        private Action _escapingStateMsg;
        private Action<float> _takingDamageMsg;
        private Action _jumpingMsg;
        private Action _attackJumpingMsg;
        private Action _attackDirectMsg;
        private Action _attackBottomMsg;
        private Action _onDeadMsg;
        private Action _onLostSightEnemyMsg;
        private Action<float> _idlingTimerMsg;
        private Action<float> _restingTimerMsg;
        private Action<float> _searchingTimerMsg;
        private Action<string> _onDetectionEnemyMsg;
        private Action<InteractableObjectBehavior> _onHitEnemyMsg;

        #endregion


        #region Fields

        private float _sqrBackJumpDistance;
        private float _sqrAttackDirectDistance;
        private float _sqrAttackBottomDistance;
        private float _sqrAttackJumpMaxDistance;
        private float _sqrAttackJumpMinDistance;
        private float _sqrChasingBrakingMinDistance;
        private float _sqrChasingBrakingMaxDistance;
        private float _sqrBattleCirclingDistance;
        private float _sqrChasingTurnDistanceNearTarget;

        public HellHoundStats Stats;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _sqrBackJumpDistance = Stats.BackJumpDistance * Stats.BackJumpDistance;
            _sqrAttackDirectDistance = Stats.AttackDirectDistance * Stats.AttackDirectDistance;
            _sqrAttackBottomDistance = Stats.AttackBottomDistance * Stats.AttackBottomDistance;
            _sqrAttackJumpMaxDistance = Stats.AttackJumpMaxDistance * Stats.AttackJumpMaxDistance;
            _sqrAttackJumpMinDistance = Stats.AttackJumpMinDistance * Stats.AttackJumpMinDistance;
            _sqrChasingBrakingMinDistance = Stats.ChasingBrakingMinDistance * Stats.ChasingBrakingMinDistance;
            _sqrChasingBrakingMaxDistance = Stats.ChasingBrakingMaxDistance * Stats.ChasingBrakingMaxDistance;
            _sqrBattleCirclingDistance = Stats.BattleCirclingMaxDistance * Stats.BattleCirclingMaxDistance;
            _sqrChasingTurnDistanceNearTarget = Stats.ChasingTurnDistanceNearTarget * Stats.ChasingTurnDistanceNearTarget;

            DebugMessages(Stats.DebugMessages);
    }

        #endregion


        #region Methods

        #region EnemyModel

        public bool Filter(Collider collider)
        {
            if (!collider.isTrigger)
            {
                InteractableObjectBehavior behaviorIO = collider.GetComponent<InteractableObjectBehavior>();
                return behaviorIO != null
                    && (behaviorIO.Type == InteractableObjectType.Player || collider.CompareTag(TagManager.PLAYER));
            }
            return false;
        }

        public void OnDetectionEnemy(Collider collider, HellHoundModel model)
        {
            _onDetectionEnemyMsg?.Invoke(collider.name);
            model.ChasingTarget = collider.transform;
            model.BehaviourState = SetState(BehaviourState.Chasing, model);
        }

        public void OnLostEnemy(Collider collider, HellHoundModel model)
        {
            if (collider.transform.Equals(model.ChasingTarget))
            {
                _onLostSightEnemyMsg?.Invoke();
                model.ChasingTarget = null;
            }
        }

        public void OnHitEnemy(Collider collider, HellHoundModel model)
        {
            InteractableObjectBehavior enemy = collider.GetComponent<InteractableObjectBehavior>();

            if (enemy != null)
            {
                DealDamage(enemy, Stats.Damage);
            }
            else
            {
                Debug.LogError(this + " not found enemy InteractableObjectBehavior");
            }

            model.AttackCollider.enabled = false;
        }

        public void OnAttackStateEnter(HellHoundModel model)
        {
            model.IsAttacking = true;
            model.AttackCollider.enabled = true;
        }

        public void OnAttackStateExit(HellHoundModel model)
        {
            model.IsAttacking = false;
            model.AttackCollider.enabled = false;
        }

        public void Act(HellHoundModel model)
        {
            float rotateDirection = GetRotateDirection(model.Transform, ref model.RotatePosition1, ref model.RotatePosition2);
            model.Animator.SetFloat("RotateDirection", rotateDirection);
            model.Animator.SetFloat("MovementSpeed", model.NavMeshAgent.velocity.sqrMagnitude);
            if (model.JumpingAttackTimer > 0) model.JumpingAttackTimer -= Time.deltaTime;

            switch (model.BehaviourState)
            {
                case BehaviourState.None:

                    _noneStateMsg?.Invoke();

                    model.NavMeshAgent.SetDestination(model.Transform.position);

                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);

                    if (rollDice < Stats.RoamingChance)
                    {
                        selectedState = SetState(BehaviourState.Roaming, model);
                    }
                    else
                    {
                        rollDice = Random.Range(1, 100);

                        if (rollDice < Stats.RestingChance)
                        {
                            selectedState = SetState(BehaviourState.Resting, model);
                        }
                        else
                        {
                            selectedState = SetState(BehaviourState.Idling, model);
                        }
                    }
                    model.BehaviourState = selectedState;

                    break;

                case BehaviourState.Roaming:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.Idling:

                    model.StateTimer -= Time.deltaTime;
                    if(model.StateTimer <= 0)
                    { 
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.Chasing:

                    if (model.ChasingTarget == null)
                    {
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }
                    else
                    {
                        if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                        {
                            model.BehaviourState = SetState(BehaviourState.Escaping, model);
                        }
                        else
                        {
                            model.NavMeshAgent.SetDestination(model.ChasingTarget.position);

                            float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Transform.position).sqrMagnitude;
                            if (sqrDistanceToEnemy <= _sqrChasingTurnDistanceNearTarget)
                            {
                                model.Transform.rotation = model.IsAttacking ?
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.AttacksTurnSpeed) :
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);
                            }

                            ImproveBraking(model, Stats.ChasingBraking);

                            if (!model.IsAttacking)
                            {
                                sqrDistanceToEnemy = (model.ChasingTarget.position - model.Transform.position).sqrMagnitude;

                                if (sqrDistanceToEnemy < _sqrBackJumpDistance)
                                {
                                    model.BehaviourState = SetState(BehaviourState.JumpingBack, model);
                                }
                                else if (sqrDistanceToEnemy < _sqrAttackBottomDistance)
                                {
                                    AttackBottom(model.Animator);
                                }
                                else if (sqrDistanceToEnemy < _sqrAttackDirectDistance)
                                {
                                    AttackDirect(model.Animator);
                                }
                                else if (model.JumpingAttackTimer <= 0 && 
                                    sqrDistanceToEnemy < _sqrAttackJumpMaxDistance && sqrDistanceToEnemy > _sqrAttackJumpMinDistance)
                                {
                                    model.BehaviourState = SetState(BehaviourState.JumpingAttack, model);
                                }
                            }
                        }
                    }

                    break;

                case BehaviourState.JumpingAttack:

                    if (!model.IsAttacking)
                    { 
                        model.BehaviourState = SetState(BehaviourState.Chasing, model);
                        model.JumpingAttackTimer = Stats.AttackJumpCooldown;
                    }

                    break;

                case BehaviourState.JumpingBack:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = model.ChasingTarget != null ?
                            SetState(BehaviourState.BattleCircling, model) :
                            SetState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.BattleCircling:

                    if (model.ChasingTarget == null)
                    {
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }
                    else
                    {
                        if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                        {
                            model.BehaviourState = SetState(BehaviourState.Escaping, model);
                        }
                        else
                        {
                            model.Transform.rotation = SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);

                            model.StateTimer -= Time.deltaTime;
                            float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Transform.position).sqrMagnitude;

                            if (model.StateTimer <= 0 || sqrDistanceToEnemy > _sqrBattleCirclingDistance)
                            {
                                model.BehaviourState = SetState(BehaviourState.Chasing, model);
                            }
                            else if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                            {
                                Vector3 navMeshPoint;
                                if (!RandomBorderCircleNavMeshPoint(model.ChasingTarget.position, Stats.BattleCirclingRadius, Stats.BattleCirclingRadius * 2, out navMeshPoint)
                                    || !model.NavMeshAgent.SetDestination(navMeshPoint))
                                {
                                    Debug.LogWarning(this + ": not found NavMesh point in case BehaviourState.BattleCircling");
                                    model.BehaviourState = SetState(BehaviourState.Chasing, model);
                                }
                            }
                        }
                    }

                    break;

                case BehaviourState.Escaping:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        if (model.ChasingTarget == null)
                        {
                            model.BehaviourState = SetState(BehaviourState.None, model);
                        }
                        else
                        {
                            Vector3 navMeshPoint;
                            bool isSetDestination = false;

                            for (int i = 0; i < 100; i++)
                            {
                                if (!RandomBorderCircleNavMeshPoint(model.ChasingTarget.position, Stats.EscapeDistance, Stats.EscapeDistance * 2, out navMeshPoint))
                                {
                                    Debug.LogError(this + ": not found NavMesh in case BehaviourState.Escaping");
                                    model.BehaviourState = SetState(BehaviourState.Chasing, model);
                                    break;
                                }

                                isSetDestination = model.NavMeshAgent.SetDestination(navMeshPoint);
                                if (isSetDestination) break;
                            }

                            if (!isSetDestination)
                            {
                                Debug.LogWarning(this + ": impossible to reach the destination point in case BehaviourState.Escaping");
                                model.BehaviourState = SetState(BehaviourState.Chasing, model);
                            }
                        }
                    }
                    
                    break;

                case BehaviourState.Resting:

                    model.StateTimer -= Time.deltaTime;
                    if (model.StateTimer <= 0)
                    {
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.Searching:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        Vector3 navMeshPoint;
                        bool isSetDestination = false;

                        for (int i = 0; i < 100; i++)
                        {
                            if (!RandomInsideSphereNavMeshPoint(model.SpawnPoint, Stats.WanderingRadius, Stats.WanderingRadius * 2, out navMeshPoint))
                            {
                                Debug.LogError(this + ": not found NavMesh in case BehaviourState.Searching");
                                model.BehaviourState = SetState(BehaviourState.Idling, model);
                                break;
                            }

                            isSetDestination = model.NavMeshAgent.SetDestination(navMeshPoint);
                            if (isSetDestination) break;
                        }

                        if (!isSetDestination)
                        {
                            Debug.LogWarning(this + ": impossible to reach the destination point in case BehaviourState.Searching");
                            model.BehaviourState = SetState(BehaviourState.Idling, model);
                        }
                    }

                    model.StateTimer -= Time.deltaTime;
                    if (model.StateTimer <= 0)
                    {
                        model.BehaviourState = SetState(BehaviourState.None, model);
                    }

                    break;
            }
        }

        #endregion

        #region SetStates

        /// <summary>Used to correctly change states. Allows correctly exit a current state and enter a new state</summary>
        private BehaviourState SetState(BehaviourState newState, HellHoundModel model)
        {
            //exit from current state
            switch (model.BehaviourState)
            {
                case BehaviourState.None:
                    break;

                case BehaviourState.Roaming:
                    break;

                case BehaviourState.Idling:
                    break;

                case BehaviourState.Chasing:
                    break;

                case BehaviourState.JumpingBack:
                    break;

                case BehaviourState.BattleCircling:
                    model.Animator.SetBool("BattleCircling", false);
                    break;

                case BehaviourState.Escaping:
                    break;

                case BehaviourState.Resting:
                    model.Animator.SetTrigger("RestingEnd");
                    break;

                case BehaviourState.Searching:
                    break;

                case BehaviourState.JumpingAttack:
                    break;
            }

            //enter to new state
            switch (newState)
            {
                case BehaviourState.None:
                    return BehaviourState.None;

                case BehaviourState.Roaming:
                    return SetRoamingState(model.NavMeshAgent, model.SpawnPoint, ref model.StateTimer);

                case BehaviourState.Idling:
                    return SetIdlingState(ref model.StateTimer);

                case BehaviourState.Chasing:
                    return SetChasingState(model.NavMeshAgent);

                case BehaviourState.JumpingBack:
                    return SetJumpingBackState(model.NavMeshAgent, model.Animator, model.Transform);

                case BehaviourState.BattleCircling:
                    return SetBattleCirclingState(model.NavMeshAgent, model.Animator, model.ChasingTarget.position, ref model.StateTimer);

                case BehaviourState.Escaping:
                    return SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);

                case BehaviourState.Resting:
                    return SetRestingState(model.Animator, ref model.StateTimer);

                case BehaviourState.Searching:
                    return SetSearchingState(model.NavMeshAgent, model.SpawnPoint, ref model.StateTimer);

                case BehaviourState.JumpingAttack:
                    return SetJumpingAttackState(model, model.ChasingTarget.position);

                default: return newState;
            }
        }

        private BehaviourState SetIdlingState(ref float timer)
        {
            _idlingStateMsg?.Invoke();

            timer = Random.Range(Stats.IdlingMinTime, Stats.IdlingMaxTime);
            _idlingTimerMsg?.Invoke(timer);

            return BehaviourState.Idling;
        }

        private BehaviourState SetRoamingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint, ref float timer)
        {
            _roamingStateMsg?.Invoke();

            navMeshAgent.speed = Stats.MaxRoamingSpeed;
            navMeshAgent.acceleration = Stats.Acceleration;
            navMeshAgent.updateRotation = true;

            Vector3 navMeshPoint;
            bool isSetDestination = false;

            for (int i = 0; i < 100; i++)
            {
                if (!RandomInsideSphereNavMeshPoint(spawnPoint, Stats.WanderingRadius, Stats.WanderingRadius * 2, out navMeshPoint))
                {
                    Debug.LogError(this + ": not found NavMesh in SetRoamingState method");
                    return SetIdlingState(ref timer);
                }

                isSetDestination = navMeshAgent.SetDestination(navMeshPoint);
                if (isSetDestination) break;
            }

            if (!isSetDestination)
            {
                Debug.LogWarning(this + ": impossible to reach the destination point in SetRoamingState method");
                return SetIdlingState(ref timer);
            }

            return BehaviourState.Roaming;
        }

        private BehaviourState SetRestingState(Animator animator, ref float timer)
        {
            _restingStateMsg?.Invoke();

            timer = Random.Range(Stats.RestingMinTime, Stats.RestingMaxTime);
            _restingTimerMsg?.Invoke(timer);

            if (Random.Range(1, 100) < 50) animator.SetTrigger("RestingSit");
            else animator.SetTrigger("RestingLie");

            return BehaviourState.Resting;
        }

        private BehaviourState SetChasingState(NavMeshAgent navMeshAgent)
        {
            _chasingStateMsg?.Invoke();

            navMeshAgent.updateRotation = true;
            navMeshAgent.stoppingDistance = Stats.StoppingDistance;
            navMeshAgent.acceleration = Stats.Acceleration;
            navMeshAgent.speed = Stats.MaxChasingSpeed;

            return BehaviourState.Chasing;
        }

        private BehaviourState SetJumpingBackState(NavMeshAgent navMeshAgent, Animator animator, Transform transform)
        {
            _backJumpingStateMsg?.Invoke();

            Vector3 jumpDirection = (transform.position - navMeshAgent.destination).normalized;
            Vector3 jumpPoint = transform.position + jumpDirection * Stats.BackJumpLength;

            NavMeshHit navMeshHit;
            if (!NavMesh.SamplePosition(jumpPoint, out navMeshHit, Stats.BackJumpLength * 2, NavMesh.AllAreas))
            {
                Debug.LogWarning(this + "not found NavMesh point in SetJumpingBackState method");
                return SetChasingState(navMeshAgent);
            }

            navMeshAgent.updateRotation = false;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.speed = Stats.BackJumpSpeed;
            navMeshAgent.acceleration = Stats.BackJumpSpeed * 10;
            navMeshAgent.SetDestination(navMeshHit.position);
            animator.Play("JumpBack");

            return BehaviourState.JumpingBack;
        }

        private BehaviourState SetBattleCirclingState(NavMeshAgent navMeshAgent, Animator animator, Vector3 chasingTargetPosition, ref float timer)
        {
            _battleCirclingStateMsg?.Invoke();

            timer = Random.Range(Stats.BattleCirclingMinTime, Stats.BattleCirclingMaxTime);
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.updateRotation = false;
            navMeshAgent.speed = Stats.BattleCirclingSpeed;
            navMeshAgent.acceleration = Stats.Acceleration;

            Vector3 navMeshPoint;
            if (!RandomBorderCircleNavMeshPoint(chasingTargetPosition, Stats.BattleCirclingRadius, Stats.BattleCirclingRadius * 2, out navMeshPoint)
                || !navMeshAgent.SetDestination(navMeshPoint))
            {
                Debug.LogWarning(this + ": not found NavMesh point in SetBattleCirclingState method");
                return SetChasingState(navMeshAgent);
            }

            animator.SetBool("BattleCircling", true);
            return BehaviourState.BattleCircling;
        }

        private BehaviourState SetEscapingState(NavMeshAgent navMeshAgent, Vector3 chasingTargetPosition)
        {
            _escapingStateMsg?.Invoke();

            navMeshAgent.speed = Stats.EscapingSpeed;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.updateRotation = true;

            Vector3 navMeshPoint;
            bool isSetDestination = false;

            for (int i = 0; i < 100; i++)
            {
                if (!RandomBorderCircleNavMeshPoint(chasingTargetPosition, Stats.EscapeDistance, Stats.EscapeDistance * 2, out navMeshPoint))
                {
                    Debug.LogError(this + ": not found NavMesh in SetEscapingState method");
                    return SetChasingState(navMeshAgent);
                }

                isSetDestination = navMeshAgent.SetDestination(navMeshPoint);
                if (isSetDestination) break;
            }

            if (!isSetDestination)
            {
                Debug.LogWarning(this + ": impossible to reach the destination point in SetEscapingState method");
                return SetChasingState(navMeshAgent);
            }

            return BehaviourState.Escaping;
        }

        private BehaviourState SetSearchingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint, ref float timer)
        {
            _searchingStateMsg?.Invoke();

            timer = Stats.SearchingTime;
            _searchingTimerMsg?.Invoke(timer);

            navMeshAgent.speed = Stats.SearchingSpeed;
            navMeshAgent.acceleration = Stats.Acceleration;

            Vector3 navMeshPoint;
            bool isSetDestination = false;

            for (int i = 0; i < 100; i++)
            {
                if (!RandomInsideSphereNavMeshPoint(spawnPoint, Stats.WanderingRadius, Stats.WanderingRadius * 2, out navMeshPoint))
                {
                    Debug.LogError(this + ": not found NavMesh in SetSearchingState method");
                    return SetIdlingState(ref timer);
                }

                isSetDestination = navMeshAgent.SetDestination(navMeshPoint);
                if (isSetDestination) break;
            }

            if (!isSetDestination)
            {
                Debug.LogWarning(this + ": impossible to reach the destination point in SetSearchingState method");
                return SetIdlingState(ref timer);
            }

            return BehaviourState.Searching;
        }

        private BehaviourState SetJumpingAttackState(HellHoundModel model, Vector3 targetPosition)
        {
            _attackJumpingMsg?.Invoke();

            model.NavMeshAgent.speed = Stats.AttackJumpSpeed;
            model.NavMeshAgent.acceleration = Stats.AttackJumpSpeed * 10;
            model.NavMeshAgent.SetDestination(targetPosition);
            model.Animator.Play("AttackJump");
            model.IsAttacking = true;

            return BehaviourState.JumpingAttack;
        }

        private void Jump(Animator animator)
        {
            _jumpingMsg?.Invoke();
            animator.Play("Jump");
        }

        private void AttackDirect(Animator animator)
        {
            _attackDirectMsg?.Invoke();
            animator.Play("AttackDirect");
        }

        private void AttackBottom(Animator animator)
        {
            _attackBottomMsg?.Invoke();
            animator.Play("AttackBottom");
        }

        #endregion

        #region Helpers

        private Quaternion SmoothTurn(Vector3 targetDirection, Vector3 forward, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, speed, 0.0f);
            newDirection.y = forward.y;
            return Quaternion.LookRotation(newDirection);
        }

        /// <summary>Current health in percent</summary>
        private float CurrentHealthPercent(float currentHealth)
        {
            return currentHealth * 100 / BaseStats.MainStats.MaxHealth;
        }

        /// <summary>Get the direction of the turn</summary>
        /// <param name="transform">HellHoundModel transform</param>
        /// <param name="rotatePosition1">Previous rotation value</param>
        /// <param name="rotatePosition2">Current rotation value</param>
        /// <returns>If value is negative turn goes left</returns>
        private float GetRotateDirection(Transform transform, ref float rotatePosition1, ref float rotatePosition2)
        {
            rotatePosition1 = rotatePosition2;
            rotatePosition2 = transform.rotation.eulerAngles.y;
            return rotatePosition2 - rotatePosition1;
        }

        /// <summary>Searches for a random NavMesh point on the boundary of a circle. If successful saves it to out variable</summary>
        /// <param name="center">center of circle</param>
        /// <param name="radius">radius of circle</param>
        /// <param name="searchDistance">the length of the ray that searches for NavMesh</param>
        /// <param name="navMeshPoint">out parameter for save the found NavMesh point</param>
        /// <returns>true if successful</returns>
        private bool RandomBorderCircleNavMeshPoint(Vector3 center, float radius, float searchDistance, out Vector3 navMeshPoint)
        {
            navMeshPoint = default;
            NavMeshHit navMeshHit;
            Vector3 randomPoint;
            Vector2 randomPoint2D;
            Vector2 center2D = new Vector2(center.x, center.z);

            for (int i = 0; i < 100; i++)
            {
                randomPoint2D = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * radius + center2D;
                randomPoint = new Vector3(randomPoint2D.x, center.y, randomPoint2D.y);

                if (NavMesh.SamplePosition(randomPoint, out navMeshHit, searchDistance, NavMesh.AllAreas))
                {
                    navMeshPoint = navMeshHit.position;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Searches for a random NavMesh point inside sphere. If successful saves it to out variable</summary>
        /// <param name="center">center of sphere</param>
        /// <param name="radius">radius of sphere</param>
        /// <param name="searchDistance">the length of the ray that searches for NavMesh</param>
        /// <param name="navMeshPoint">out parameter for save the found NavMesh point</param>
        /// <returns>true if successful</returns>
        private bool RandomInsideSphereNavMeshPoint(Vector3 center, float radius, float searchDistance, out Vector3 navMeshPoint)
        {
            navMeshPoint = default;
            NavMeshHit navMeshHit;
            Vector3 randomPoint;

            for (int i = 0; i < 100; i++)
            {
                randomPoint = Random.insideUnitSphere * radius + center;
                if (NavMesh.SamplePosition(randomPoint, out navMeshHit, searchDistance, NavMesh.AllAreas))
                {
                    navMeshPoint = navMeshHit.position;
                    return true;
                }
            }
            return false;
        }

        /// <summary>improve braking (the dog brakes more gently)</summary>
        /// <param name="switcher">on/off braking</param>
        private void ImproveBraking(HellHoundModel model, bool switcher)
        {
            if (switcher)
            {
                float sqrDistance = (model.ChasingTarget.position - model.Transform.position).sqrMagnitude;
                model.NavMeshAgent.speed =
                    sqrDistance <= _sqrChasingBrakingMaxDistance ?
                    (sqrDistance < _sqrChasingBrakingMinDistance ?
                    Stats.ChasingBrakingMinSpeed :
                    sqrDistance * Stats.ChasingBrakingSpeedRate) :
                    Stats.MaxChasingSpeed;
            }
        }

        /// <summary>Subscribes to message delegates</summary>
        /// <param name="switcher">on/off debug messages</param>
        private void DebugMessages(bool switcher)
        {
            if (switcher)
            {
                _noneStateMsg = () => Debug.Log("State selection");
                _idlingStateMsg = () => Debug.Log("The dog is idling");
                _roamingStateMsg = () => Debug.Log("The dog is roaming");
                _restingStateMsg = () => Debug.Log("The dog is resting");
                _chasingStateMsg = () => Debug.Log("The dog is chasing");
                _backJumpingStateMsg = () => Debug.Log("The dog is jumping back");
                _battleCirclingStateMsg = () => Debug.Log("The dog is battle circling");
                _searchingStateMsg = () => Debug.Log("The dog is searching");
                _escapingStateMsg = () => Debug.Log("The dog is escaping");
                _takingDamageMsg = (health) => Debug.Log("The dog is taking damage. CurrentHealth = " + health);
                _jumpingMsg = () => Debug.Log("The dog is jumping");
                _attackJumpingMsg = () => Debug.Log("The dog is jumping attack");
                _attackDirectMsg = () => Debug.Log("The dog is attacking direct");
                _attackBottomMsg = () => Debug.Log("The dog is attacking bottom");
                _onDeadMsg = () => Debug.Log("Hell hound is dead");
                _idlingTimerMsg = (timer) => Debug.Log("Idling time = " + timer);
                _restingTimerMsg = (timer) => Debug.Log("Resting timer = " + timer);
                _searchingTimerMsg = (timer) => Debug.Log("Searching timer = " + timer);
                _onHitEnemyMsg = (enemy) => Debug.Log("The dog is deal damage to " + enemy);
                _onDetectionEnemyMsg = (colliderName) => Debug.Log("The dog noticed " + colliderName);
                _onLostSightEnemyMsg = () => Debug.Log("The dog lost sight of the target");
            }
        }

        #endregion

        #endregion


        #region EnemyData

        public override void TakeDamage(EnemyModel model, Damage damage)
        {
            HellHoundModel hellHoundModel = model as HellHoundModel;
            base.TakeDamage(model, damage);

            _takingDamageMsg?.Invoke(model.CurrentHealth);
            hellHoundModel.Animator.SetTrigger("TakeDamage");

            if (model.IsDead)
            {
                _onDeadMsg?.Invoke();
                hellHoundModel.Animator.SetTrigger("Dead");
            }

            if (hellHoundModel.ChasingTarget == null
                && (hellHoundModel.BehaviourState == BehaviourState.Roaming 
                || hellHoundModel.BehaviourState == BehaviourState.Idling
                || hellHoundModel.BehaviourState == BehaviourState.Resting))
            {
                hellHoundModel.BehaviourState = SetState(BehaviourState.Searching, hellHoundModel);
            }
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            Damage countDamage = Services.SharedInstance.AttackService
                .CountDamage(damage, BaseStats.MainStats, enemy.transform.GetMainParent().gameObject.GetInstanceID());

            _onHitEnemyMsg?.Invoke(enemy);
            enemy.TakeDamageEvent(countDamage);
        }

        #endregion
    }
}
