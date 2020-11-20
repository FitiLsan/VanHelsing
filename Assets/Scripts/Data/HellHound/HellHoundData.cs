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
            BattleCircling
        }

        #endregion


        #region Constants

        private const float ROAMING_CHANCE = 75.0f;

        private const float IDLING_MIN_TIME = 5.0f;
        private const float IDLING_MAX_TIME = 10.0f;

        private const float CHASING_TURN_SPEED_NEAR_TARGET = 0.1f;
        private const float CHASING_TURN_DISTANCE_TO_TARGET = 3.0f;

        private const float CHASING_BRAKING_MAX_DISTANCE = 6.0f;
        private const float CHASING_BRAKING_MIN_DISTANCE = 2.0f;
        private const float CHASING_BRAKING_MIN_SPEED = 2.0f;
        private const float CHASING_BRAKING_SPEED_RATE = 1.5f;

        #endregion


        #region Fields

        private float sqrtBackJumpDistance;
        private float sqrtAttackTorsoMaxDistance;
        private float sqrtAttackJumpMaxDistance;
        private float sqrtAttackJumpMinDistance;
        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles

        public HellHoundData()
        {
            Stats.WanderingRadius = 50.0f;
            Stats.DetectionRadius = 5.0f;
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
            Stats.AttackTorsoMaxDistance = 1.5f;
            Stats.AttackJumpMaxDistance = 3.0f;
            Stats.AttackJumpMinDistance = 2.5f;
            Stats.BattleCirclingRadius = 5.0f;
            Stats.BattleCirclingSpeed = 3.0f;
            Stats.BattleCirclingMinTime = 1.0f;
            Stats.BattleCirclingMaxTime = 3.0f;
    }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            sqrtBackJumpDistance = Stats.BackJumpDistance * Stats.BackJumpDistance;
            sqrtAttackTorsoMaxDistance = Stats.AttackTorsoMaxDistance * Stats.AttackTorsoMaxDistance;
            sqrtAttackJumpMaxDistance = Stats.AttackJumpMaxDistance * Stats.AttackJumpMaxDistance;
            sqrtAttackJumpMinDistance = Stats.AttackJumpMinDistance * Stats.AttackJumpMinDistance;
        }

        #endregion


        #region Methods

        public void Act(HellHoundModel model)
        {
            float sqrDistance;

            //for tests:
            if (Input.GetKeyDown(KeyCode.J)) Jump(model.Animator);
            if (Input.GetKeyDown(KeyCode.K)) model.BehaviourState = SetJumpingBackState(model);
            if (Input.GetKeyDown(KeyCode.G)) AttackDirect(model.Animator, model.AttackCollider);
            if (Input.GetKeyDown(KeyCode.H)) AttackBottom(model.Animator, model.AttackCollider);
            if (Input.GetKeyDown(KeyCode.U)) model.BehaviourState = SetBattleCirclingState(model);


            float rotateDirection = GetRotateDirection(model.Transform, ref model.RotatePosition1, ref model.RotatePosition2);
            model.Animator.SetFloat("RotateDirection", rotateDirection);
            model.Animator.SetFloat("MovementSpeed", model.NavMeshAgent.velocity.sqrMagnitude);

            switch (model.BehaviourState)
            {
                case BehaviourState.None:

                    Debug.Log("State selection");

                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);

                    if (rollDice < ROAMING_CHANCE)
                    {
                        selectedState = SetRoamingState(model.NavMeshAgent, model.SpawnPoint);
                    }
                    else
                    {
                        selectedState = SetIdlingState(ref model.IdlingTimer);
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

                    model.IdlingTimer -= Time.deltaTime;
                    if(model.IdlingTimer <= 0)
                    { 
                        model.BehaviourState = BehaviourState.None;
                    }

                    break;

                case BehaviourState.Chasing:

                    model.NavMeshAgent.SetDestination(model.ChasingTarget.position);

                    if (model.NavMeshAgent.remainingDistance <= CHASING_TURN_DISTANCE_TO_TARGET)
                    {
                        model.Transform.rotation = model.IsAttacking?
                            SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, CHASING_TURN_SPEED_NEAR_TARGET) :
                            SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, 0.5f);
                    }

                    ////for improve braking:
                    //sqrDistance = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;
                    //model.NavMeshAgent.speed =
                    //    sqrDistance <= CHASING_BRAKING_MAX_DISTANCE * CHASING_BRAKING_MAX_DISTANCE ?
                    //    (sqrDistance < CHASING_BRAKING_MAX_DISTANCE * CHASING_BRAKING_MAX_DISTANCE ?
                    //    CHASING_BRAKING_MIN_SPEED :
                    //    sqrDistance * CHASING_BRAKING_SPEED_RATE) :
                    //    Stats.MaxChasingSpeed;

                    if (!model.IsAttacking)
                    {
                        sqrDistance = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;

                        if (sqrDistance < sqrtBackJumpDistance)
                        {
                            model.BehaviourState = SetJumpingBackState(model);
                        }
                        else if (sqrDistance <= sqrtAttackTorsoMaxDistance)
                        {
                            AttackDirect(model.Animator, model.AttackCollider);
                        }
                        else if (sqrDistance < sqrtAttackJumpMaxDistance && sqrDistance > sqrtAttackJumpMinDistance)
                        {
                            AttackJump(model.Animator, model.AttackCollider);
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

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        Func<Vector3> randomCirclePoint = () => new Vector3(Random.value - 0.5f, model.ChasingTarget.position.y, Random.value - 0.5f).normalized * Stats.BattleCirclingRadius + model.ChasingTarget.position;

                        if (!RandomDestination(model.NavMeshAgent, randomCirclePoint, Stats.BattleCirclingRadius * 2, 1))
                        {
                            Debug.LogWarning(this + "not found NavMesh point in case BehaviourState.BattleCircling");
                            model.BehaviourState = SetChasingState(model.NavMeshAgent);
                        }
                    }

                    model.Transform.rotation = SmoothTurn(model.ChasingTarget.position - model.Rigidbody.position, model.Transform.forward, CHASING_TURN_SPEED_NEAR_TARGET);

                    model.BattleCirclingTimer -= Time.deltaTime;
                    if (model.BattleCirclingTimer <= 0)
                    {
                        model.Animator.SetBool("BattleCircling", false);
                        model.BehaviourState = SetChasingState(model.NavMeshAgent);
                    }

                    break;
            }
        }

        private BehaviourState SetBattleCirclingState(HellHoundModel model)
        {
            Debug.Log("The dog is battle circling");

            model.NavMeshAgent.stoppingDistance = 0;
            model.NavMeshAgent.updateRotation = false;
            model.BattleCirclingTimer = Random.Range(Stats.BattleCirclingMinTime, Stats.BattleCirclingMaxTime);
            model.NavMeshAgent.speed = Stats.BattleCirclingSpeed;
            model.NavMeshAgent.acceleration = Stats.Acceleration;

            Func<Vector3> randomCirclePoint = () => new Vector3(Random.value - 0.5f, model.ChasingTarget.position.y, Random.value - 0.5f).normalized * Stats.BattleCirclingRadius + model.ChasingTarget.position;
            if (!RandomDestination(model.NavMeshAgent, randomCirclePoint, Stats.BattleCirclingRadius * 2, 1))
            {
                Debug.LogWarning(this + "not found NavMesh point in SetBattleCirclingState()");
                return SetChasingState(model.NavMeshAgent);
            }

            model.Animator.SetBool("BattleCircling", true);
            return BehaviourState.BattleCircling;
        }

        private BehaviourState SetIdlingState(ref float idlingTimer)
        {
            Debug.Log("The dog is idling");

            idlingTimer = Random.Range(IDLING_MIN_TIME, IDLING_MAX_TIME);
            Debug.Log("Idling time = " + idlingTimer);

            return BehaviourState.Idling;
        }

        private BehaviourState SetRoamingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint)
        {
            Debug.Log("The dog is roaming");

            navMeshAgent.speed = Stats.MaxRoamingSpeed;

            Func<Vector3> randomSpherePointFunc = () => Random.insideUnitSphere * Stats.WanderingRadius + spawnPoint; ;
            if (!RandomDestination(navMeshAgent, randomSpherePointFunc, Stats.WanderingRadius * 2, 100))
            {
                Debug.LogError(this + ": impossible to reach the destination point");
            }

            return BehaviourState.Roaming;
        }

        public BehaviourState SetChasingState(NavMeshAgent navMeshAgent)
        {
            Debug.Log("The dog is chasing");

            navMeshAgent.updateRotation = true;
            navMeshAgent.stoppingDistance = Stats.StoppingDistance;
            navMeshAgent.acceleration = Stats.Acceleration;
            navMeshAgent.speed = Stats.MaxChasingSpeed;
            return BehaviourState.Chasing;
        }

        private BehaviourState SetJumpingBackState(HellHoundModel model)
        {
            Debug.Log("The dog is jumping back");

            Vector3 jumpDirection = (model.Rigidbody.position - model.NavMeshAgent.destination).normalized;
            Vector3 jumpPoint = model.Rigidbody.position + jumpDirection * Stats.BackJumpLength;

            NavMeshHit navMeshHit;
            if (!NavMesh.SamplePosition(jumpPoint, out navMeshHit, Stats.BackJumpLength*2, NavMesh.AllAreas))
            { 
                Debug.LogWarning(this + "not found NavMesh point in SetJumpingBackState method");
                return SetChasingState(model.NavMeshAgent);
            }

            model.NavMeshAgent.updateRotation = false;
            model.NavMeshAgent.stoppingDistance = 0;
            model.NavMeshAgent.speed = Stats.BackJumpSpeed;
            model.NavMeshAgent.acceleration = Stats.BackJumpSpeed * 10;
            model.NavMeshAgent.SetDestination(navMeshHit.position);
            model.Animator.SetTrigger("JumpBack");

            return BehaviourState.JumpingBack;
        }

        private void Jump(Animator animator)
        {
            Debug.Log("The dog is jumping");
            animator.SetTrigger("Jump");
        }

        private void AttackJump(Animator animator, Collider attackCollider)
        {
            Debug.Log("The dog is jumping attack");
            animator.SetTrigger("AttackJump");
        }

        private void AttackDirect(Animator animator, Collider attackCollider)
        {
            Debug.Log("The dog is attacking torso");
            animator.SetTrigger("AttackDirect");
        }

        private void AttackBottom(Animator animator, Collider attackCollider)
        {
            Debug.Log("The dog is attacking legs");
            animator.SetTrigger("AttackBottom");
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

        private bool RandomDestination(NavMeshAgent navMeshAgent, Func<Vector3> randomPointFunc, float searchDistance, int attemptsAmount)
        {
            Vector3 navMeshPoint;
            for (int i = 0; i < attemptsAmount; i++)
            {
                if (!SearchNavMesh(randomPointFunc, searchDistance, out navMeshPoint))
                {
                    Debug.LogError(this + ": could not find NavMesh point");
                    return false;
                }
                if (navMeshAgent.SetDestination(navMeshPoint))
                {
                    return true;
                }
            }
            return false;
        }

        private bool SearchNavMesh(Func<Vector3> randomPointFunc, float searchDistance, out Vector3 navMeshPoint)
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
            return false;
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
            base.TakeDamage(model, damage);

            if (model.IsDead)
            {
                Debug.Log("Hell hound is dead");
                (model as HellHoundModel).OnDead();
            }
        }

        #endregion
    }
}
