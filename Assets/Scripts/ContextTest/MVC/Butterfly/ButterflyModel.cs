using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class ButterflyModel
    {
        #region Fields

        public float TimeLeft = 1.0f;
        public float TimeElapsed = 0.0f;
        public float TimeElapsedAfterStateChange = 0.0f;
        public float TimeElapsedAfterStartFleeing = 0.0f;

        public float CurrentHealth;
        public bool IsDead;

        public List<Transform> DangerousObjects;
        public Vector3 NextCoord;
        public ButterflyData.BehaviourState ButterflyState;

        #endregion


        #region Properties

        public ButterflyData ButterflyData { get; }
        public GameObject Butterfly { get; }
        public Transform ButterflyTransform { get; }
        public Rigidbody ButterflyRigidbody { get; }
        public Vector3 ButterflyStartPosition { get; }

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject prefab, ButterflyData butterflyData)
        {
            if (prefab.GetComponent<Rigidbody>() != null)
            {
                ButterflyData = butterflyData;
                Butterfly = prefab;
                ButterflyTransform = prefab.transform;
                ButterflyRigidbody = prefab.GetComponent<Rigidbody>();
                ButterflyStartPosition = prefab.transform.position;

                CurrentHealth = butterflyData.ButterflyStruct.MaxHealth;
                IsDead = butterflyData.ButterflyStruct.IsDead;

                DangerousObjects = new List<Transform>();
                NextCoord = butterflyData.RandomNextCoord(ButterflyTransform, ButterflyStartPosition, DangerousObjects);
                if (butterflyData.ButterflyStruct.CanIdle)
                {
                    ButterflyState = ButterflyData.BehaviourState.Idling;
                }
                else
                {
                    ButterflyState = ButterflyData.BehaviourState.Roaming;
                }
            }
            else
            {
                Debug.LogError("Invalid Rabbit prefab: no Rigidbody");
            }
        }

        #endregion


        #region Metods

        public void Execute()
        {
            if (!IsDead)
            {
                ButterflyData.Act(this);
            }
        }

        #endregion
    }
}