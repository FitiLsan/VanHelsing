using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewHellHound", menuName = "CreateData/HellHound", order = 2)]
    public sealed class HellHoundData : EnemyData
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
            Searching
        }

        #endregion


        #region Debug messages

        private Action NoneStateMsg;
        private Action IdlingStateMsg;
        private Action RoamingStateMsg;
        private Action RestingStateMsg;
        private Action ChasingStateMsg;
        private Action BackJumpingStateMsg;
        private Action BattleCirclingStateMsg;
        private Action SearchingStateMsg;
        private Action EscapingStateMsg;
        private Action TakingDamageMsg;
        private Action JumpingMsg;
        private Action AttackJumpingMsg;
        private Action AttackDirectMsg;
        private Action AttackBottomMsg;
        private Action OnDeadMsg;
        private Action OnLostSightEnemyMsg;
        private Action<float> IdlingTimerMsg;
        private Action<float> RestingTimerMsg;
        private Action<float> SearchingTimerMsg;
        private Action<string> OnDetectionEnemyMsg;
        private Action<InteractableObjectBehavior> OnHitEnemyMsg;

        #endregion


        #region Fields

        private float sqrBackJumpDistance;
        private float sqrAttackDirectDistance;
        private float sqrAttackBottomDistance;
        private float sqrAttackJumpMaxDistance;
        private float sqrAttackJumpMinDistance;
        private float sqrChasingBrakingMinDistance;
        private float sqrChasingBrakingMaxDistance;
        private float sqrBattleCirclingDistance;
        private float sqrChasingTurnDistanceNearTarget;

        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles

        public HellHoundData()
        {
            Stats.DebugMessages = false;
            Stats.WanderingRadius = 50.0f;
            Stats.DetectionRadius = 20.0f;
            Stats.PercentEscapeHealth = 30.0f;
            Stats.MaxRoamingSpeed = 2.0f;
            Stats.MaxChasingSpeed = 10.0f;
            Stats.AngularSpeed = 450.0f;
            Stats.Acceleration = 10.0f;
            Stats.BaseOffsetByY = -0.05f;
            Stats.StoppingDistance = 1.5f;
            Stats.JumpingSpeedRate = 1.2f;
            Stats.BackJumpAnimationSpeedRate = 2.0f;
            Stats.BackJumpAnimationIntensity = 0.5f;
            Stats.BackJumpLength = 1.5f;
            Stats.BackJumpSpeed = 5.0f;
            Stats.BackJumpDistance = 1.0f;
            Stats.AttackDirectDistance = 1.5f;
            Stats.AttackBottomDistance = 1.25f;
            Stats.AttackJumpMaxDistance = 3.0f;
            Stats.AttackJumpMinDistance = 2.5f;
            Stats.AttacksTurnSpeed = 0.5f;
            Stats.BattleCirclingRadius = 3.0f;
            Stats.BattleCirclingSpeed = 3.0f;
            Stats.BattleCirclingMinTime = 1.0f;
            Stats.BattleCirclingMaxTime = 3.0f;
            Stats.BattleCirclingMaxDistance = 5.0f;
            Stats.RestingMinTime = 30.0f;
            Stats.RestingMaxTime = 60.0f;
            Stats.RoamingChance = 75.0f;
            Stats.RestingChance = 20.0f;
            Stats.IdlingMinTime = 5.0f;
            Stats.IdlingMaxTime = 10.0f;
            Stats.SearchingTime = 45.0f;
            Stats.SearchingSpeed = 5.0f;
            Stats.EscapeDistance = 30.0f;
            Stats.EscapingSpeed = 7.0f;
            Stats.ChasingBraking = false;
            Stats.ChasingBrakingMaxDistance = 6.0f;
            Stats.ChasingBrakingMinDistance = 2.0f;
            Stats.ChasingBrakingMinSpeed = 2.0f;
            Stats.ChasingBrakingSpeedRate = 0.5f;
            Stats.ChasingTurnSpeedNearTarget = 0.1f;
            Stats.ChasingTurnDistanceNearTarget = 3.0f;
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            sqrBackJumpDistance = Stats.BackJumpDistance * Stats.BackJumpDistance;
            sqrAttackDirectDistance = Stats.AttackDirectDistance * Stats.AttackDirectDistance;
            sqrAttackBottomDistance = Stats.AttackBottomDistance * Stats.AttackBottomDistance;
            sqrAttackJumpMaxDistance = Stats.AttackJumpMaxDistance * Stats.AttackJumpMaxDistance;
            sqrAttackJumpMinDistance = Stats.AttackJumpMinDistance * Stats.AttackJumpMinDistance;
            sqrChasingBrakingMinDistance = Stats.ChasingBrakingMinDistance * Stats.ChasingBrakingMinDistance;
            sqrChasingBrakingMaxDistance = Stats.ChasingBrakingMaxDistance * Stats.ChasingBrakingMaxDistance;
            sqrBattleCirclingDistance = Stats.BattleCirclingMaxDistance * Stats.BattleCirclingMaxDistance;
            sqrChasingTurnDistanceNearTarget = Stats.ChasingTurnDistanceNearTarget * Stats.ChasingTurnDistanceNearTarget;

            DebugMessages(Stats.DebugMessages);
    }

        #endregion


        #region Methods

        #region EnemyModel

        public bool DetectionFilter(Collider collider)
        {
            InteractableObjectBehavior IOBehavior = collider.GetComponent<InteractableObjectBehavior>();
            return !collider.isTrigger
                && collider.CompareTag(TagManager.PLAYER) && IOBehavior != null && IOBehavior.Type == InteractableObjectType.Player;
        }

        public void OnDetectionEnemy(Collider collider, HellHoundModel model)
        {
            OnDetectionEnemyMsg?.Invoke(collider.name);
            model.ChasingTarget = collider.transform;
            model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
        }

        public void OnLostSightEnemy(Collider collider, HellHoundModel model)
        {
            if (collider.transform.Equals(model.ChasingTarget))
            {
                OnLostSightEnemyMsg?.Invoke();
                model.ChasingTarget = null;
            }
        }

        public bool OnHitEnemyFilter(Collider collider)
        {
            InteractableObjectBehavior IOBehavior = collider.GetComponent<InteractableObjectBehavior>();
            return !collider.isTrigger
                && collider.CompareTag(TagManager.PLAYER) && IOBehavior != null && IOBehavior.Type == InteractableObjectType.Player;
        }

        public void OnHitEnemy(Collider collider, HellHoundModel model)
        {
            Damage damage = new Damage()
            {
                PhysicalDamage = Stats.PhysicalDamage,
                StunProbability = Stats.StunProbability
            };

            InteractableObjectBehavior enemy = collider.gameObject.GetComponent<InteractableObjectBehavior>();
            OnHitEnemyMsg?.Invoke(enemy);

            if (enemy != null) model.WeaponIO.DealDamageEvent(enemy, damage);
            else Debug.LogError(this + " not found enemy InteractableObjectBehavior");

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

            switch (model.BehaviourState)
            {
                case BehaviourState.None:

                    NoneStateMsg?.Invoke();

                    model.NavMeshAgent.SetDestination(model.Rigidbody.position);

                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);

                    if (rollDice < Stats.RoamingChance)
                    {
                        selectedState = ChangeState(BehaviourState.Roaming, model);
                    }
                    else
                    {
                        rollDice = Random.Range(1, 100);

                        if (rollDice < Stats.RestingChance)
                        {
                            selectedState = ChangeState(BehaviourState.Resting, model);
                        }
                        else
                        {
                            selectedState = ChangeState(BehaviourState.Idling, model);
                        }
                    }
                    model.BehaviourState = selectedState;

                    break;

                case BehaviourState.Roaming:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.Idling:

                    model.Timer -= Time.deltaTime;
                    if(model.Timer <= 0)
                    { 
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }

                    break;

                case BehaviourState.Chasing:

                    if (model.ChasingTarget == null)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }
                    else
                    {
                        if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                        {
                            model.BehaviourState = ChangeState(BehaviourState.Escaping, model);
                        }
                        else
                        {
                            model.NavMeshAgent.SetDestination(model.ChasingTarget.position);

                            float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;
                            if (sqrDistanceToEnemy <= sqrChasingTurnDistanceNearTarget)
                            {
                                model.Transform.rotation = model.IsAttacking ?
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.AttacksTurnSpeed) :
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);
                            }

                            ImproveBraking(model, Stats.ChasingBraking);

                            if (!model.IsAttacking)
                            {
                                sqrDistanceToEnemy = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;

                                if (sqrDistanceToEnemy < sqrBackJumpDistance)
                                {
                                    model.BehaviourState = ChangeState(BehaviourState.JumpingBack, model);
                                }
                                else if (sqrDistanceToEnemy < sqrAttackBottomDistance)
                                {
                                    AttackBottom(model.Animator);
                                }
                                else if (sqrDistanceToEnemy < sqrAttackDirectDistance)
                                {
                                    AttackDirect(model.Animator);
                                }
                                else if (sqrDistanceToEnemy < sqrAttackJumpMaxDistance && sqrDistanceToEnemy > sqrAttackJumpMinDistance)
                                {
                                    AttackJump(model.Animator);
                                }
                            }
                        }
                    }

                    break;

                case BehaviourState.JumpingBack:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.BattleCircling, model);
                    }

                    break;

                case BehaviourState.BattleCircling:

                    if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.Escaping, model);
                    }
                    else
                    {
                        model.Transform.rotation = SmoothTurn(model.ChasingTarget.position - model.Rigidbody.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);

                        model.Timer -= Time.deltaTime;
                        float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;

                        if (model.Timer <= 0 || sqrDistanceToEnemy > sqrBattleCirclingDistance)
                        {
                            model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                        }
                        else if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                        {
                            Vector3 navMeshPoint;
                            if (!RandomBorderCircleNavMeshPoint(model.ChasingTarget.position, Stats.BattleCirclingRadius, Stats.BattleCirclingRadius * 2, out navMeshPoint)
                                || !model.NavMeshAgent.SetDestination(navMeshPoint))
                            {
                                Debug.LogWarning(this + ": not found NavMesh point in case BehaviourState.BattleCircling");
                                model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                            }
                        }
                    }

                    break;

                case BehaviourState.Escaping:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        if (model.ChasingTarget == null)
                        {
                            model.BehaviourState = ChangeState(BehaviourState.None, model);
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
                                    model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                                    break;
                                }

                                isSetDestination = model.NavMeshAgent.SetDestination(navMeshPoint);
                                if (isSetDestination) break;
                            }

                            if (!isSetDestination)
                            {
                                Debug.LogError(this + ": impossible to reach the destination point in case BehaviourState.Escaping");
                                model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                            }
                        }
                    }
                    
                    break;

                case BehaviourState.Resting:

                    model.Timer -= Time.deltaTime;
                    if (model.Timer <= 0)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
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
                                model.BehaviourState = ChangeState(BehaviourState.Idling, model);
                                break;
                            }

                            isSetDestination = model.NavMeshAgent.SetDestination(navMeshPoint);
                            if (isSetDestination) break;
                        }

                        if (!isSetDestination)
                        {
                            Debug.LogError(this + ": impossible to reach the destination point in case BehaviourState.Searching");
                            model.BehaviourState = ChangeState(BehaviourState.Idling, model);
                        }
                    }

                    model.Timer -= Time.deltaTime;
                    if (model.Timer <= 0)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }

                    break;
            }
        }

        #endregion

        #region SetStates

        /// <summary>Used to correctly change states. Allows correctly terminate a state and enter a new one</summary>
        private BehaviourState ChangeState(BehaviourState newState, HellHoundModel model)
        {
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
            }

            switch (newState)
            {
                case BehaviourState.None:
                    return BehaviourState.None;

                case BehaviourState.Roaming:
                    return SetRoamingState(model.NavMeshAgent, model.SpawnPoint, ref model.Timer);

                case BehaviourState.Idling:
                    return SetIdlingState(ref model.Timer);

                case BehaviourState.Chasing:
                    return SetChasingState(model.NavMeshAgent);

                case BehaviourState.JumpingBack:
                    return SetJumpingBackState(model.NavMeshAgent, model.Animator, model.Rigidbody);

                case BehaviourState.BattleCircling:
                    return SetBattleCirclingState(model.NavMeshAgent, model.Animator, model.ChasingTarget.position, ref model.Timer);

                case BehaviourState.Escaping:
                    return SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);

                case BehaviourState.Resting:
                    return SetRestingState(model.Animator, ref model.Timer);

                case BehaviourState.Searching:
                    return SetSearchingState(model.NavMeshAgent, model.SpawnPoint, ref model.Timer);

                default: return newState;
            }

        }

        private BehaviourState SetIdlingState(ref float timer)
        {
            IdlingStateMsg?.Invoke();

            timer = Random.Range(Stats.IdlingMinTime, Stats.IdlingMaxTime);
            IdlingTimerMsg?.Invoke(timer);

            return BehaviourState.Idling;
        }

        private BehaviourState SetRoamingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint, ref float timer)
        {
            RoamingStateMsg?.Invoke();

            navMeshAgent.speed = Stats.MaxRoamingSpeed;
            navMeshAgent.acceleration = Stats.Acceleration;

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
                Debug.LogError(this + ": impossible to reach the destination point in SetRoamingState method");
                return SetIdlingState(ref timer);
            }

            return BehaviourState.Roaming;
        }

        private BehaviourState SetRestingState(Animator animator, ref float timer)
        {
            RestingStateMsg?.Invoke();

            timer = Random.Range(Stats.RestingMinTime, Stats.RestingMaxTime);
            RestingTimerMsg?.Invoke(timer);

            if (Random.Range(1, 100) < 50) animator.SetTrigger("RestingSit");
            else animator.SetTrigger("RestingLie");

            return BehaviourState.Resting;
        }

        private BehaviourState SetChasingState(NavMeshAgent navMeshAgent)
        {
            ChasingStateMsg?.Invoke();

            navMeshAgent.updateRotation = true;
            navMeshAgent.stoppingDistance = Stats.StoppingDistance;
            navMeshAgent.acceleration = Stats.Acceleration;
            navMeshAgent.speed = Stats.MaxChasingSpeed;

            return BehaviourState.Chasing;
        }

        private BehaviourState SetJumpingBackState(NavMeshAgent navMeshAgent, Animator animator, Rigidbody rigidbody)
        {
            BackJumpingStateMsg?.Invoke();

            Vector3 jumpDirection = (rigidbody.position - navMeshAgent.destination).normalized;
            Vector3 jumpPoint = rigidbody.position + jumpDirection * Stats.BackJumpLength;

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
            BattleCirclingStateMsg?.Invoke();

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
            EscapingStateMsg?.Invoke();

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
                Debug.LogError(this + ": impossible to reach the destination point in SetEscapingState method");
                return SetChasingState(navMeshAgent);
            }

            return BehaviourState.Escaping;
        }

        private BehaviourState SetSearchingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint, ref float timer)
        {
            SearchingStateMsg?.Invoke();

            timer = Stats.SearchingTime;
            SearchingTimerMsg?.Invoke(timer);

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
                Debug.LogError(this + ": impossible to reach the destination point in SetSearchingState method");
                return SetIdlingState(ref timer);
            }

            return BehaviourState.Searching;
        }
    

        private void Jump(Animator animator)
        {
            JumpingMsg?.Invoke();
            animator.Play("Jump");
        }

        private void AttackJump(Animator animator)
        {
            AttackJumpingMsg?.Invoke();
            animator.Play("AttackJump");
        }

        private void AttackDirect(Animator animator)
        {
            AttackDirectMsg?.Invoke();
            animator.Play("AttackDirect");
        }

        private void AttackBottom(Animator animator)
        {
            AttackBottomMsg?.Invoke();
            animator.Play("AttackBottom");
        }

        #endregion

        #region Helpers

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

        private float CurrentHealthPercent(float currentHealth)
        {
            return currentHealth * 100 / BaseStats.MainStats.MaxHealth;
        }

        private Quaternion SmoothTurn(Vector3 targetDirection, Vector3 forward, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, speed, 0.0f);
            newDirection.y = forward.y;
            return Quaternion.LookRotation(newDirection);
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

        /// <summary>improve braking (the dog brakes more gently)</summary>
        /// <param name="switcher">on/off braking</param>
        private void ImproveBraking(HellHoundModel model, bool switcher)
        {
            if (switcher)
            {
                float sqrDistance = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;
                model.NavMeshAgent.speed =
                    sqrDistance <= sqrChasingBrakingMaxDistance ?
                    (sqrDistance < sqrChasingBrakingMinDistance ?
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
                NoneStateMsg = () => Debug.Log("State selection");
                IdlingStateMsg = () => Debug.Log("The dog is idling");
                RoamingStateMsg = () => Debug.Log("The dog is roaming");
                RestingStateMsg = () => Debug.Log("The dog is resting");
                ChasingStateMsg = () => Debug.Log("The dog is chasing");
                BackJumpingStateMsg = () => Debug.Log("The dog is jumping back");
                BattleCirclingStateMsg = () => Debug.Log("The dog is battle circling");
                SearchingStateMsg = () => Debug.Log("The dog is searching");
                EscapingStateMsg = () => Debug.Log("The dog is escaping");
                TakingDamageMsg = () => Debug.Log("The dog is taking damage");
                JumpingMsg = () => Debug.Log("The dog is jumping");
                AttackJumpingMsg = () => Debug.Log("The dog is jumping attack");
                AttackDirectMsg = () => Debug.Log("The dog is attacking direct");
                AttackBottomMsg = () => Debug.Log("The dog is attacking bottom");
                OnDeadMsg = () => Debug.Log("Hell hound is dead");
                IdlingTimerMsg = (timer) => Debug.Log("Idling time = " + timer);
                RestingTimerMsg = (timer) => Debug.Log("Resting timer = " + timer);
                SearchingTimerMsg = (timer) => Debug.Log("Searching timer = " + timer);
                OnHitEnemyMsg = (enemy) => Debug.Log("The dog is deal damage to " + enemy);
                OnDetectionEnemyMsg = (colliderName) => Debug.Log("The dog noticed " + colliderName);
                OnLostSightEnemyMsg = () => Debug.Log("The dog lost sight of the target");
            }
        }

        #endregion

        #endregion


        #region EnemyData

        public override void TakeDamage(EnemyModel model, Damage damage)
        {
            HellHoundModel hellHoundModel = model as HellHoundModel;
            base.TakeDamage(model, damage);

            TakingDamageMsg?.Invoke();
            hellHoundModel.Animator.SetTrigger("TakeDamage");

            if (model.IsDead)
            {
                OnDeadMsg?.Invoke();
                hellHoundModel.Animator.SetTrigger("Dead");
                hellHoundModel.NavMeshAgent.enabled = false;
            }

            if (hellHoundModel.ChasingTarget == null
                && (hellHoundModel.BehaviourState == BehaviourState.Roaming 
                || hellHoundModel.BehaviourState == BehaviourState.Idling
                || hellHoundModel.BehaviourState == BehaviourState.Resting))
            {
                hellHoundModel.BehaviourState = ChangeState(BehaviourState.Searching, hellHoundModel);
            }
        }

        #endregion
    }
}
