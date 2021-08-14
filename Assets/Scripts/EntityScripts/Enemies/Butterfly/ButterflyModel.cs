using System;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel : EnemyModel
    {
        #region Fields
        
        private Vector3 _playerPreviousPosition;
        private readonly ButterflyData _data;
        public ButterflyState State;
        public Vector3 TargetCoordinates;
        public readonly Transform Transform;
        public readonly Rigidbody Rigidbody;
        public readonly GameObject GameObject;
        public readonly GameObject Player;
        
        #endregion


        #region Properties

        public float PlayerSpeed => (Player.transform.position -
                _playerPreviousPosition).magnitude /
            (Time.deltaTime.Equals(0) ? float.Epsilon : Time.deltaTime);

        #endregion

        
        #region ClassLifeCycles

        public ButterflyModel(ButterflyData data, GameObject butterfly, GameObject player)
        {
            Rigidbody = butterfly.GetComponent<Rigidbody>() ??
                throw new ArgumentNullException(nameof(butterfly));
            Transform = butterfly.transform;
            CurrentHealth = data.BaseStats.MaxHealth;
            IsDead = false;
            State = ButterflyState.Calm;
            GameObject = butterfly;
            Player = player;
            _data = data;
        }

        #endregion


        #region Methods
        
        public override void Execute()
        {
            _data.Act(this);
            _playerPreviousPosition = Player.transform.position;
        }

        public override EnemyStats GetStats() =>
            _data.BaseStats;

        public override void DoSmth(string how)
        {
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                _data.TakeDamage(this, damage);
            }
        }

        #endregion
    }
}
