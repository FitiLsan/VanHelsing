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
            Chasing
        }

        #endregion


        #region Constants

        private const float ROAMING_CHANCE = 75.0f;
        private const float MIN_IDLING_TIME = 5.0f;
        private const float MAX_IDLING_TIME = 10.0f;
        private const float TURN_SPEED_NEAR_CHASING_TARGET = 0.1f;

        #endregion


        #region Fields

        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles

        public HellHoundData()
        {
            Stats.WanderingRadius = 50.0f;
            Stats.DetectionRadius = 5.0f;
            Stats.RoamingSpeed = 2.0f;
            Stats.RunSpeed = 20.0f;
    }

        #endregion


        #region Methods

        public void Act(HellHoundModel model)
        {
            switch (model.BehaviourState)
            {
                case BehaviourState.None:

                    Debug.Log("State selection");
                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);

                    if (rollDice < ROAMING_CHANCE)
                    {
                        selectedState = BehaviourState.Roaming;
                    }
                    else
                    {
                        selectedState = BehaviourState.Idling;
                    }

                    model.BehaviourState = selectedState;
                    OnChangeState(model);

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

                    Vector3 ChasingTarget = model.ChasingTarget.transform.position;
                    model.NavMeshAgent.SetDestination(ChasingTarget);

                    if (model.NavMeshAgent.velocity == Vector3.zero)
                    {
                        model.HellHound.transform.rotation = Turn(ChasingTarget, model.Transform.position, model.Transform.forward);
                    }

                    if (Vector3.Distance(ChasingTarget, model.Transform.position) > Stats.DetectionRadius + 2)
                    {
                        Debug.Log("The dog is stopped chasing");
                        Debug.Log(Vector3.Distance(ChasingTarget, model.Transform.position) + " > " + Stats.DetectionRadius);
                        model.BehaviourState = BehaviourState.None;
                        model.ChasingTarget = null;
                    }

                    break;
            }
        }

        private void OnChangeState(HellHoundModel model)
        {
            Debug.Log("Behaviour state change on " + model.BehaviourState);

            switch (model.BehaviourState)
            {
                case BehaviourState.Roaming:

                    model.NavMeshAgent.speed = Stats.RoamingSpeed;

                    bool isFoundRoamingPath = false;
                    Vector3 destinationPoint;

                    for (int i = 0; i < 100; i++)
                    {
                        if (!NewDestinationPoint(model.SpawnPoint, out destinationPoint))
                        {
                            Debug.LogError(model.HellHound.name + ": could not find NavMesh point");
                            break;
                        }
                        isFoundRoamingPath = model.NavMeshAgent.SetDestination(destinationPoint);
                        if (isFoundRoamingPath) break;
                    }

                    if (!isFoundRoamingPath) Debug.LogError(model.HellHound.name + ": impossible to reach the destination point");

                    break;

                case BehaviourState.Idling:

                    model.IdlingTimer = Random.Range(MIN_IDLING_TIME, MAX_IDLING_TIME);
                    Debug.Log("Set idlingTimer on " + model.IdlingTimer);

                    break;

            }
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

        private Quaternion Turn(Vector3 targetPoint, Vector3 position, Vector3 forward)
        {
            Vector3 targetDirection = targetPoint - position;
            Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, TURN_SPEED_NEAR_CHASING_TARGET, 0.0f);
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
