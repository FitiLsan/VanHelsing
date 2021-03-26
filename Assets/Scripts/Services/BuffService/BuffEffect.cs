using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct BuffEffect
    {
        [Tooltip("Active Rage value between 0 and 100.")]
        public Buff Buff;
        [Tooltip("Effect value between 0 and 10000.")]
        [Range(0.0f, 10000.0f)]
        public float Value;
        public bool IsTicking;
    }
}

