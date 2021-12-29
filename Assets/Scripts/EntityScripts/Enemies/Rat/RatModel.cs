using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class RatModel : EnemyModel
    {

        #region Fields

        public float TimeLeft = 1.0f;
        public float TimeElapsed = 0.0f;
        public float TimeElapsedAfterStateChange = 0.0f;
        public float TimeElapsedAfterStartFleeing = 0.0f;

        public List<Transform> DangerousObjects;
        public Vector3 NextCoord;
        public RatData.BehaviourState RatState;

        #endregion


        #region Properties

        public RatData RatData { get; }
        public GameObject Rat { get; }
        public Transform RatTransform { get; }
        public Rigidbody RatRigidbody { get; }
        public Vector3 RatStartPosition { get; }

        #endregion


        #region ClassLifeCycle

        public RatModel(GameObject prefab, RatData ratData)
        {
            if (prefab.GetComponent<Rigidbody>() != null)
            {
                RatData = ratData;
                Rat = prefab;
                RatTransform = prefab.transform;
                RatRigidbody = prefab.GetComponent<Rigidbody>();
                RatStartPosition = prefab.transform.position;

                CurrentHealth = ratData.BaseStats.MaxHealth;
                IsDead = false;

                DangerousObjects = new List<Transform>();
                NextCoord = ratData.RandomNextCoord(RatTransform, RatStartPosition, DangerousObjects);
                if (ratData.RatStats.CanIdle)
                {
                    RatState = RatData.BehaviourState.Idling;
                }
                else
                {
                    RatState = RatData.BehaviourState.Roaming;
                }
            }
            else
            {
                Debug.LogError("Invalid Rat prefab: no Rigidbody");
            }
        }

        #endregion


        #region NpcModel

        public override void Execute()
        {
            if (!IsDead)
            {
                RatData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return RatData.BaseStats;
        }


        public override void DoSmth(string how)
        {
            //RatData.Do(how);
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                RatData.TakeDamage(this, damage);
            }
        }

        #endregion
    }
}
