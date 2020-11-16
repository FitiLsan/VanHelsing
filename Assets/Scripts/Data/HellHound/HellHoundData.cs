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

        #endregion


        #region Fields

        public HellHoundStats Stats;

        #endregion


        #region ClassLifeCycles

        public HellHoundData()
        {
            Stats.WanderingRadius = 50.0f;
        }

        #endregion


        #region Methods

        public void Act(HellHoundModel model)
        {
            //если в поле зрения собаки попадает игрок
            //то включается стадия преследования 
            //model.BehaviourState = BehaviourState.Chasing

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

                    //Chasing logic
                    Debug.Log("The dog is chasing");

                    break;
            }
        }

        public void OnChangeState(HellHoundModel model)
        {
            Debug.Log("Behaviour state change on " + model.BehaviourState);

            switch (model.BehaviourState)
            {
                case BehaviourState.Roaming:

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
