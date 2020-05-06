using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct GiantMudCrabStruct
    {
        #region Fields

        public Damage AttackDamage;

        public BaseStatsClass Stats;

        public float MoveSpeed;

        public float AttackSpeed;

        public float AttackRange;

        public float MaxHealth;

        public float CurrentHealth;

        public float PatrolDistance;

        public float TriggerDistance;

        public float DiggingDistance;

        public GameObject Prefab;

        public Vector3 SpawnPoint;

        public GameObject CrabProjectile;

        public Transform CrabMouth;

        public bool CanAttack;

        public bool IsDigIn;

        public bool IsPatrol;

        public bool IsDead;

        #endregion
    }
}

