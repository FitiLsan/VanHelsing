using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class RabbitModel : EnemyModel
    {

        #region Fields
              
        public float TimeLeft = 1.0f;
        public float TimeElapsed = 0.0f;
        public float TimeElapsedAfterStateChange = 0.0f;
        public float TimeElapsedAfterStartFleeing = 0.0f;

        public List<Transform> DangerousObjects;
        public Vector3 NextCoord;
        public RabbitData.BehaviourState RabbitState;

        #endregion


        #region Properties

        public RabbitData RabbitData { get; }
        public GameObject Rabbit { get; }
        public Transform RabbitTransform { get; }
        public Rigidbody RabbitRigidbody { get; }
        public Vector3 RabbitStartPosition { get; }

        #endregion


        #region ClassLifeCycle

        public RabbitModel(GameObject prefab, RabbitData rabbitData)
        {
            if (prefab.GetComponent<Rigidbody>() != null)
            {
                RabbitData = rabbitData;
                Rabbit = prefab;
                RabbitTransform = prefab.transform;
                RabbitRigidbody = prefab.GetComponent<Rigidbody>();
                RabbitStartPosition = prefab.transform.position;

                CurrentHealth = rabbitData.BaseStats.MainStats.MaxHealth;
                IsDead = false;

                DangerousObjects = new List<Transform>();
                NextCoord = rabbitData.RandomNextCoord(RabbitTransform, RabbitStartPosition, DangerousObjects);
                if (rabbitData.RabbitStats.CanIdle)
                {
                    RabbitState = RabbitData.BehaviourState.Idling;
                }
                else
                {
                    RabbitState = RabbitData.BehaviourState.Roaming;
                }
            }
            else
            {
                Debug.LogError("Invalid Rabbit prefab: no Rigidbody");
            }
        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
           
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                RabbitData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return RabbitData.BaseStats;
        }


        public override void DoSmth(string how)
        {
            //RabbitData.Do(how);
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                RabbitData.TakeDamage(this, damage);
            }
        }

        public override void OnTearDown()
        {
        }

        #endregion
    }
}
