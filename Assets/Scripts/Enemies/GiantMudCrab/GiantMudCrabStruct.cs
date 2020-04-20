using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct GiantMudCrabStruct
    {
        #region Fields

        public DamageStruct AttackDamage;

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

        public bool ShouldDigIn;

        public bool IsPatrol;

        public bool IsChase;

        public bool IsDead;

        #endregion
    }
}

