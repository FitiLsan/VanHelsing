using System;
using BeastHunter;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel : EnemyModel
    {
        #region Fields
        
        public ButterflyState State;
        public Vector3 TargetCoordinates;
        public readonly Transform Transform;
        public readonly Rigidbody Rigidbody;
        private readonly ButterflyData data;
        public readonly GameObject GameObject;
        private float playerSpeed;

        #endregion


        #region Properties

        
        public GameObject Player { get; set; }
        
        public Vector3 PlayerPreviousPosition { get; set; }

        public float PlayerSpeed => (Player.transform.position -
                PlayerPreviousPosition).magnitude /
            (Time.deltaTime.Equals(0) ? float.Epsilon : Time.deltaTime);

        #endregion

        
        #region ClassLifeCycles

        public ButterflyModel(ButterflyData data, GameObject gameObject)
        {
            Rigidbody = gameObject.GetComponent<Rigidbody>() ??
                throw new ArgumentNullException(nameof(gameObject));
            Transform = gameObject.transform;
            CurrentHealth = data.BaseStats.MaxHealth;
            IsDead = false;
            State = ButterflyState.Calm;
            this.data = data;
            GameObject = gameObject;
        }

        #endregion


        #region Methods

        
        public override void Execute()
        {
            data.Act(this);
        }

        public override EnemyStats GetStats()
        {
            return data.BaseStats;
        }

        public override void DoSmth(string how)
        {
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                data.TakeDamage(this, damage);
            }
        }

        #endregion
    }
}
