//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public sealed class HellHoundModel : EnemyModel
    {
        private const float MIN_IDLING_TIME = 5.0f;
        private const float MAX_IDLING_TIME = 10.0f;

        #region Fields

        private HellHoundData hellHoundData;
        HellHoundData.BehaviourState behaviourState;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Rigidbody rigidbody;

        public Vector3 SpawnPoint;
        public Vector3 TargetPoint;
        public float IdlingTimer;

        #endregion


        #region Properties

        public GameObject HellHound { get; }

        #endregion


        #region ClassLifeCycle

        public HellHoundModel(GameObject gameObject, HellHoundData hellHoundData)
        {
            this.hellHoundData = hellHoundData;
            HellHound = gameObject;
            animator = HellHound.GetComponent<Animator>();
            navMeshAgent = HellHound.GetComponent<NavMeshAgent>();
            rigidbody = HellHound.GetComponent<Rigidbody>();

            SpawnPoint = HellHound.transform.position;
            TargetPoint = SpawnPoint;
            behaviourState = HellHoundData.BehaviourState.Roaming;

            CurrentHealth = this.hellHoundData.BaseStats.MainStats.MaxHealth;
            IsDead = false;
        }

        #endregion

        void NewTargetPoint()
        {
            float wanderingRadius = hellHoundData.Stats.WanderingRadius;

            Vector3 randomPoint;
            NavMeshHit navMeshHit = new NavMeshHit();
            int i = 0;
            bool result = false;
            while (!result)
            {
                randomPoint = Random.insideUnitSphere * wanderingRadius + SpawnPoint;
                result = NavMesh.SamplePosition(randomPoint, out navMeshHit, wanderingRadius * 2, NavMesh.AllAreas);

                if (i++ > 100) //infinite loop protection
                {
                    Debug.LogWarning(HellHound.name + ": could not find NavMesh");
                    break;
                }
            }

            if (result) TargetPoint = navMeshHit.position;
            else TargetPoint = SpawnPoint;
        }

        void Roaming()
        {
            Debug.Log("The dog is roaming");
            int i = 0;
            bool result = false;
            while (!result)
            {
                NewTargetPoint();
                result = navMeshAgent.SetDestination(TargetPoint);

                if (i++ > 100) //infinite loop protection
                {
                    Debug.LogError(HellHound.name + ": impossible to reach the target point");
                    break;
                }
            }
        }

        void Idling()
        {
            if (IdlingTimer <= 0)
            {
                IdlingTimer = Random.Range(MIN_IDLING_TIME, MAX_IDLING_TIME);
                Debug.Log("The dog is idling");
                Debug.Log("Iidling timer = " + IdlingTimer);
            }
            else
            {
                IdlingTimer -= Time.deltaTime;
                if (IdlingTimer <= 0)
                {
                    behaviourState = HellHoundData.BehaviourState.Roaming;
                    Debug.Log("The dog finished idling");
                }
            }
        }

        #region Methods

        public void OnDead()
        {
            animator.SetBool("IsDead", true);
        }

        #endregion


        #region EnemyModel

        public override void DoSmth(string how)
        {
            hellHoundData.Do(how);
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                //hellHoundData.Act(this);

                switch (behaviourState)
                {
                    case HellHoundData.BehaviourState.Roaming:
                        if (rigidbody.position == TargetPoint)
                        {
                            if (Random.Range(1, 100) < 75) Roaming();
                            else behaviourState = HellHoundData.BehaviourState.Idling;
                        }
                        break;
                    case HellHoundData.BehaviourState.Idling:
                        Idling();
                        break;
                }
            }
        }

        public override EnemyStats GetStats()
        {
            return hellHoundData.BaseStats;
        }

        public override void OnAwake()
        {
            
        }

        public override void OnTearDown()
        {
            
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                hellHoundData.TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
