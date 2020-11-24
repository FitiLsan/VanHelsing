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
        private Action<float> IdlingTimerMsg;
        private Action<float> RestingTimerMsg;
        private Action<float> SearchingTimerMsg;

        #endregion


        #region Fields

        private float sqrtBackJumpDistance;
        private float sqrtAttackDirectDistance;
        private float sqrtAttackBottomDistance;
        private float sqrtAttackJumpMaxDistance;
        private float sqrtAttackJumpMinDistance;
        private float sqrtEscapeDistance;
        private float sqrChasingBrakingMinDistance;
        private float sqrChasingBrakingMaxDistance;
        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles

        public HellHoundData()
        {
            Stats.DebugMessages = false;
            Stats.WanderingRadius = 50.0f;
            Stats.DetectionRadius = 20.0f;
            Stats.EscapeDistance = 30.0f;
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
            Stats.BattleCirclingRadius = 3.0f;
            Stats.BattleCirclingSpeed = 3.0f;
            Stats.BattleCirclingMinTime = 1.0f;
            Stats.BattleCirclingMaxTime = 3.0f;
            Stats.AttacksTurnSpeed = 0.5f;
            Stats.RestingMinTime = 30.0f;
            Stats.RestingMaxTime = 60.0f;
            Stats.RoamingChance = 75.0f;
            Stats.RestingChance = 10.0f;
            Stats.IdlingMinTime = 5.0f;
            Stats.IdlingMaxTime = 10.0f;
            Stats.SearchingTime = 45.0f;
            Stats.SearchingSpeed = 5.0f;
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
            sqrtBackJumpDistance = Stats.BackJumpDistance * Stats.BackJumpDistance;
            sqrtAttackDirectDistance = Stats.AttackDirectDistance * Stats.AttackDirectDistance;
            sqrtAttackBottomDistance = Stats.AttackBottomDistance * Stats.AttackBottomDistance;
            sqrtAttackJumpMaxDistance = Stats.AttackJumpMaxDistance * Stats.AttackJumpMaxDistance;
            sqrtAttackJumpMinDistance = Stats.AttackJumpMinDistance * Stats.AttackJumpMinDistance;
            sqrChasingBrakingMinDistance = Stats.ChasingBrakingMinDistance * Stats.ChasingBrakingMinDistance;
            sqrChasingBrakingMaxDistance = Stats.ChasingBrakingMaxDistance * Stats.ChasingBrakingMaxDistance;

            sqrtEscapeDistance = Stats.EscapeDistance > Stats.DetectionRadius ?
                Stats.EscapeDistance * Stats.EscapeDistance :
                Stats.DetectionRadius * Stats.DetectionRadius + Stats.DetectionRadius * 0.2f;

            DebugMessages(Stats.DebugMessages);
    }

        #endregion


        #region Methods

        public void Act(HellHoundModel model)
        {
            float sqrDistance;

            float rotateDirection = GetRotateDirection(model.Transform, ref model.RotatePosition1, ref model.RotatePosition2);
            model.Animator.SetFloat("RotateDirection", rotateDirection);
            model.Animator.SetFloat("MovementSpeed", model.NavMeshAgent.velocity.sqrMagnitude);

            switch (model.BehaviourState)
            {
                case BehaviourState.None:

                    NoneStateMsg?.Invoke();

                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);

                    if (rollDice < Stats.RoamingChance)
                    {
                        selectedState = SetRoamingState(model.NavMeshAgent, model.SpawnPoint, ref model.Timer);
                    }
                    else
                    {
                        rollDice = Random.Range(1, 100);

                        if (rollDice < Stats.RestingChance)
                        {
                            selectedState = SetRestingState(model.Animator, ref model.Timer);
                        }
                        else
                        {
                            selectedState = SetIdlingState(ref model.Timer);
                        }
                    }
                    model.BehaviourState = selectedState;

                    break;

                case BehaviourState.Roaming:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = BehaviourState.None;
                    }

                    break;

                case BehaviourState.Idling:

                    model.Timer -= Time.deltaTime;
                    if(model.Timer <= 0)
                    { 
                        model.BehaviourState = BehaviourState.None;
                    }

                    break;

                case BehaviourState.Chasing:

                    if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                    {
                        model.BehaviourState = SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);
                    }
                    else
                    {
                        model.NavMeshAgent.SetDestination(model.ChasingTarget.position);

                        if (model.NavMeshAgent.remainingDistance <= Stats.ChasingTurnDistanceNearTarget)
                        {
                            model.Transform.rotation = model.IsAttacking ?
                                SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.AttacksTurnSpeed) :
                                SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);
                        }

                        ImproveBraking(model, Stats.ChasingBraking);

                        if (!model.IsAttacking)
                        {
                            sqrDistance = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;

                            if (sqrDistance < sqrtBackJumpDistance)
                            {
                                model.BehaviourState = SetJumpingBackState(model.NavMeshAgent, model.Animator, model.Rigidbody);
                            }
                            else if (sqrDistance < sqrtAttackBottomDistance)
                            {
                                AttackBottom(model.Animator);
                            }
                            else if (sqrDistance < sqrtAttackDirectDistance)
                            {
                                AttackDirect(model.Animator);
                            }
                            else if (sqrDistance < sqrtAttackJumpMaxDistance && sqrDistance > sqrtAttackJumpMinDistance)
                            {
                                AttackJump(model.Animator);
                            }
                        }
                    }

                    break;

                case BehaviourState.JumpingBack:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = SetBattleCirclingState(model);
                    }

                    break;

                case BehaviourState.BattleCircling:

                    if (CurrentHealthPercent(model.CurrentHealth) < Stats.PercentEscapeHealth && !model.IsAttacking)
                    {
                        model.Animator.SetBool("BattleCircling", false);
                        model.BehaviourState = SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);
                    }
                    else
                    {
                        if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                        {
                            Vector3 navMeshPoint;
                            if (!SearchRandomNavMeshPoint(() => RandomBorderCirclePoint(model.ChasingTarget.position, Stats.BattleCirclingRadius), Stats.BattleCirclingRadius * 2, out navMeshPoint)
                                || !model.NavMeshAgent.SetDestination(navMeshPoint))
                            {
                                Debug.LogWarning(this + ": impossible to reach the destination point in case BehaviourState.BattleCircling");
                                model.BehaviourState = SetChasingState(model.NavMeshAgent);
                            }
                        }

                        model.Transform.rotation = SmoothTurn(model.ChasingTarget.position - model.Rigidbody.position, model.Transform.forward, Stats.ChasingTurnSpeedNearTarget);

                        model.Timer -= Time.deltaTime;
                        if (model.Timer <= 0)
                        {
                            model.Animator.SetBool("BattleCircling", false);
                            model.BehaviourState = SetChasingState(model.NavMeshAgent);
                        }
                    }

                    break;

                case BehaviourState.Escaping:

                    sqrDistance = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;
                    if (sqrDistance >= sqrtEscapeDistance)
                    {
                        model.BehaviourState = BehaviourState.None;
                    }
                    else
                    {
                        if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                        {
                            Vector3 navMeshpoint;
                            if (!SearchRandomNavMeshPoint(() => RandomBorderCirclePoint(model.ChasingTarget.position, Stats.EscapeDistance), Stats.EscapeDistance * 2, out navMeshpoint)
                                || !model.NavMeshAgent.SetDestination(navMeshpoint))
                            {
                                Debug.LogWarning(this + ": impossible to reach the destination point in case BehaviourState.Escaping");
                                model.BehaviourState = SetChasingState(model.NavMeshAgent);
                            }
                        }
                    }

                    break;

                case BehaviourState.Resting:

                    model.Timer -= Time.deltaTime;
                    if (model.Timer <= 0)
                    {
                        model.Animator.SetTrigger("RestingEnd");
                        model.BehaviourState = BehaviourState.None;
                    }

                    break;

                case BehaviourState.Searching:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        Vector3 navMeshPoint;
                        for (int i = 0; i < 100; i++)
                        {
                            if (!SearchRandomNavMeshPoint(() => RandomInsideSpherePoint(model.SpawnPoint, Stats.WanderingRadius), Stats.WanderingRadius * 2, out navMeshPoint)
                                || !model.NavMeshAgent.SetDestination(navMeshPoint))
                            {
                                Debug.LogError(this + ": impossible to reach the destination point in case BehaviourState.Searching");
                                model.BehaviourState = SetIdlingState(ref model.Timer);
                            }
                        }
                    }

                    model.Timer -= Time.deltaTime;
                    if (model.Timer <= 0)
                    {
                        model.BehaviourState = BehaviourState.None;
                    }

                    break;
            }
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
            for (int i = 0; i < 100; i++)
            {
                if (SearchRandomNavMeshPoint(() => RandomInsideSpherePoint(spawnPoint, Stats.WanderingRadius), Stats.WanderingRadius * 2, out navMeshPoint)
                    && navMeshAgent.SetDestination(navMeshPoint))
                {
                    return BehaviourState.Roaming;
                }
            }

            Debug.LogError(this + ": impossible to reach the destination point in SetRoamingState method");
            return SetIdlingState(ref timer);
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

        public BehaviourState SetChasingState(NavMeshAgent navMeshAgent)
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

        private BehaviourState SetBattleCirclingState(HellHoundModel model)
        {
            BattleCirclingStateMsg?.Invoke();

            model.Timer = Random.Range(Stats.BattleCirclingMinTime, Stats.BattleCirclingMaxTime);
            model.NavMeshAgent.stoppingDistance = 0;
            model.NavMeshAgent.updateRotation = false;
            model.NavMeshAgent.speed = Stats.BattleCirclingSpeed;
            model.NavMeshAgent.acceleration = Stats.Acceleration;

            Vector3 navMeshPoint;
            if (!SearchRandomNavMeshPoint(() => RandomBorderCirclePoint(model.ChasingTarget.position, Stats.BattleCirclingRadius), Stats.BattleCirclingRadius * 2, out navMeshPoint)
                || !model.NavMeshAgent.SetDestination(navMeshPoint))
            {
                Debug.LogWarning(this + ": impossible to reach the destination point in SetBattleCirclingState method");
                return SetChasingState(model.NavMeshAgent);
            }

            model.Animator.SetBool("BattleCircling", true);
            return BehaviourState.BattleCircling;
        }

        private BehaviourState SetEscapingState(NavMeshAgent navMeshAgent, Vector3 chasingTargetPosition)
        {
            EscapingStateMsg?.Invoke();

            navMeshAgent.speed = Stats.EscapingSpeed;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.updateRotation = true;

            Vector3 navMeshpoint;
            if (!SearchRandomNavMeshPoint(() => RandomBorderCirclePoint(chasingTargetPosition, Stats.EscapeDistance), Stats.EscapeDistance * 2, out navMeshpoint)
                || !navMeshAgent.SetDestination(navMeshpoint))
            {
                Debug.LogWarning(this + ": impossible to reach the destination point in SetEscapingState method");
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
            for (int i = 0; i < 100; i++)
            {
                if (SearchRandomNavMeshPoint(() => RandomInsideSpherePoint(spawnPoint, Stats.WanderingRadius), Stats.WanderingRadius * 2, out navMeshPoint)
                    && navMeshAgent.SetDestination(navMeshPoint))
                {
                    return BehaviourState.Searching;
                }
            }

            Debug.LogError(this + ": impossible to reach the destination point in SetSearchingState method");
            return SetIdlingState(ref timer);
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

        private Vector3 RandomInsideSpherePoint(Vector3 center, float radius)
        {
            return Random.insideUnitSphere * radius + center;
        }

        private Vector3 RandomBorderCirclePoint(Vector3 center, float radius)
        {
            Vector2 center2D = new Vector2(center.x, center.z);
            Vector2 randomPoint = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * radius + center2D;
            return new Vector3(randomPoint.x, center.y, randomPoint.y);
        }

        private bool SearchRandomNavMeshPoint(Func<Vector3> randomPointFunc, float searchDistance, out Vector3 navMeshPoint)
        {
            navMeshPoint = default;
            NavMeshHit navMeshHit;
            for (int i = 0; i < 100; i++)
            {
                if (NavMesh.SamplePosition(randomPointFunc.Invoke(), out navMeshHit, searchDistance, NavMesh.AllAreas))
                {
                    navMeshPoint = navMeshHit.position;
                    return true;
                }
            }
            Debug.LogError(this + ": could not find NavMesh point");
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
                hellHoundModel.BehaviourState = SetSearchingState(hellHoundModel.NavMeshAgent, hellHoundModel.SpawnPoint, ref hellHoundModel.Timer);
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
            }
        }

        #endregion
    }
}
