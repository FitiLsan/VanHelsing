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

        public RabbitModel(GameObject objectOnScene, RabbitData data) : base(objectOnScene, data)
        {
            if (objectOnScene.GetComponent<Rigidbody>() != null)
            {
                RabbitData = data;
                Rabbit = objectOnScene;
                RabbitTransform = objectOnScene.transform;
                RabbitRigidbody = objectOnScene.GetComponent<Rigidbody>();
                RabbitStartPosition = objectOnScene.transform.position;

                DangerousObjects = new List<Transform>();
                NextCoord = data.RandomNextCoord(RabbitTransform, RabbitStartPosition, DangerousObjects);
                if (data.RabbitStats.CanIdle)
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

        public override void TakeDamage(Damage damage)
        {
            if (!CurrentStats.BaseStats.IsDead)
            {
                RabbitData.TakeDamage(this, damage);
            }
        }

        #endregion
    }
}
