using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyModel : EnemyModel
    {
        #region Fields
        
        public BehaviourStateButterfly ButterflyState;
        internal float TimeSinceTheState;

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
            if (prefab.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                ButterflyData = butterflyData;
                Butterfly = prefab;
                ButterflyRigidbody = rigidbody;
                ButterflyTransform = rigidbody.transform;
                ButterflyStartPosition = rigidbody.transform.localPosition;

                CurrentHealth = butterflyData.BaseStats.MaxHealth;
                IsDead = false;

                if (butterflyData.ButterflyStats.CanIdle)
                {
                    ButterflyState = BehaviourStateButterfly.Idle;
                }
                else
                {
                    ButterflyState = BehaviourStateButterfly.Fly;
                }
            }
            else
            {
                Debug.LogError("Invalid Butterfly prefab: no Rigidbody");
            }
        }

        #endregion


        #region Methods

        public override void DoSmth(string how)
        {
            //throw new System.NotImplementedException();
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                ButterflyData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return ButterflyData.BaseStats;
        }

        public override void TakeDamage(Damage damage)
        {
            //throw new System.NotImplementedException();
        } 

        #endregion
    }
}