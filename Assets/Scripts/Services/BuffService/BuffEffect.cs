using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct BuffEffect
    {
        public Buff Buff;
        public EffectType BuffEffectType;
        [Range(0.0f, 10000.0f)]
        public float Value;
        public bool IsTicking;
    }
}

