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
                case BehaviourState.Roaming:
                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        Debug.Log("Choice between Roaming and Idling states");
                        if (Random.Range(1, 100) < ROAMING_CHANCE) Roaming(model);
                        else model.BehaviourState = BehaviourState.Idling;
                    }
                    break;
                case BehaviourState.Idling:
                    Idling(model);
                    break;
            }
        }

        public void SetIdlingTimer(ref float timer)
        {
            timer = Random.Range(MIN_IDLING_TIME, MAX_IDLING_TIME);
            Debug.Log("Set idlingTimer on " + timer);
        }

        private void Roaming(HellHoundModel model)
        {
            Debug.Log(model.HellHound.name + " is roaming");

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

            if (result) Debug.Log("Create new path");
        }

        private void Idling(HellHoundModel model)
        {
            model.IdlingTimer -= Time.deltaTime;
            if (model.IdlingTimer <= 0)
            {
                Debug.Log(model.HellHound.name + " finished idling");
                model.BehaviourState = BehaviourState.Roaming;
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
