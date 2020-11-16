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
            Idling
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
            switch (model.BehaviourState)
            {
                case BehaviourState.None:
                    Debug.Log("State selection");
                    if (Random.Range(1, 100) < ROAMING_CHANCE)
                    {
                        model.BehaviourState = BehaviourState.Roaming;
                    }
                    else
                    {
                        model.BehaviourState = BehaviourState.Idling;
                    }
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
            }
        }

        public void OnChangeState(HellHoundModel model)
        {
            Debug.Log("State change on " + model.BehaviourState);
            switch (model.BehaviourState)
            {
                case BehaviourState.Roaming:
                    int i = 0;
                    bool result = false;
                    while (!result)
                    {
                        NewTargetPoint(model);
                        result = model.NavMeshAgent.SetDestination(model.TargetPoint);

                        if (i++ > 100) //infinite loop protection
                        {
                            Debug.LogError(model.HellHound.name + ": impossible to reach the target point");
                            break;
                        }
                    }
                    break;

                case BehaviourState.Idling:
                    model.IdlingTimer = Random.Range(MIN_IDLING_TIME, MAX_IDLING_TIME);
                    Debug.Log("Set idlingTimer on " + model.IdlingTimer);
                    break;
            }
        }

        private void NewTargetPoint(HellHoundModel model)
        {
            float wanderingRadius = Stats.WanderingRadius;
            Vector3 randomPoint;
            NavMeshHit navMeshHit = new NavMeshHit();
            int i = 0;
            bool result = false;

            while (!result)
            {
                randomPoint = Random.insideUnitSphere * wanderingRadius + model.SpawnPoint;
                result = NavMesh.SamplePosition(randomPoint, out navMeshHit, wanderingRadius * 2, NavMesh.AllAreas);

                if (i++ > 100) //infinite loop protection
                {
                    Debug.LogWarning(model.HellHound.name + ": could not find NavMesh");
                    break;
                }
            }

            if (result) model.TargetPoint = navMeshHit.position;
            else model.TargetPoint = model.SpawnPoint;

            Debug.Log("NewTargetPoint: " + model.TargetPoint);
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
