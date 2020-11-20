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
            JumpingBack
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
            Stats.AttackJumpMaxDistance = 4.0f;
            Stats.AttackJumpMinDistance = 3.5f;
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
            if (Input.GetKeyDown(KeyCode.G)) AttackTorso(model.Animator, model.AttackCollider);
            if (Input.GetKeyDown(KeyCode.H)) AttackLegs(model.Animator, model.AttackCollider);


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

                    //for improve braking:
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
                            AttackTorso(model.Animator, model.AttackCollider);
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
                        model.BehaviourState = SetChasingState(model.NavMeshAgent);
                    }

                    break;
            }
        }

        public BehaviourState SetIdlingState(ref float idlingTimer)
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

            bool isFoundRoamingPath = false;
            Vector3 destinationPoint;

            for (int i = 0; i < 100; i++)
            {
                if (!NewDestinationPoint(spawnPoint, out destinationPoint))
                {
                    Debug.LogError(this + ": could not find NavMesh point");
                    break;
                }
                isFoundRoamingPath = navMeshAgent.SetDestination(destinationPoint);
                if (isFoundRoamingPath) break;
            }

            if (!isFoundRoamingPath) Debug.LogError(this + ": impossible to reach the destination point");

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
            model.TargetPoint = model.Rigidbody.position + jumpDirection * Stats.BackJumpLength;

            model.NavMeshAgent.updateRotation = false;
            model.NavMeshAgent.stoppingDistance = 0;
            model.NavMeshAgent.speed = Stats.BackJumpSpeed;
            model.NavMeshAgent.acceleration = Stats.BackJumpSpeed * 10;
            model.NavMeshAgent.SetDestination(model.TargetPoint);
            model.Animator.SetTrigger("JumpingBack");

            return BehaviourState.JumpingBack;
        }

        private void Jump(Animator animator)
        {
            animator.SetTrigger("Jumping");
        }
        private void AttackJump(Animator animator, Collider attackCollider)
        {
            animator.SetTrigger("AttackingJump");
        }
        private void AttackTorso(Animator animator, Collider attackCollider)
        {
            Debug.Log("AttackingTorso");
            animator.SetTrigger("AttackingTorso");
        }
        private void AttackLegs(Animator animator, Collider attackCollider)
        {
            animator.SetTrigger("AttackingLegs");
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

        /// <summary>If successful sets the out parameter to a random NavMesh point in wandering radius</summary>
        /// <param name="spawnPoint">hell hound spawn point</param>
        /// <param name="destinationPoint">parameter to sets a value of random NavMesh point</param>
        /// <returns>success status</returns>
        private bool NewDestinationPoint(Vector3 spawnPoint, out Vector3 destinationPoint)
        {
            destinationPoint = default;
            float wanderingRadius = Stats.WanderingRadius;
            Vector3 randomSpherePoint;
            NavMeshHit navMeshHit;

            for (int i = 0; i < 100; i++)
            {
                randomSpherePoint = Random.insideUnitSphere * wanderingRadius + spawnPoint;

                if (NavMesh.SamplePosition(randomSpherePoint, out navMeshHit, wanderingRadius * 2, NavMesh.AllAreas))
                {
                    destinationPoint = navMeshHit.position;
                    Debug.Log("NewDestinationPoint: " + destinationPoint);
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
