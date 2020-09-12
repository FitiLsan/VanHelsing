using System;
using UnityEngine;


namespace BeastHunter
{
    public static class GlobalEventsModel
    {
        public static Action OnPlayerDie { get; set; }
        public static Action OnBossStunned { get; set; }
        public static Action OnBossHitted { get; set; }
        public static Action OnBossDie { get; set; }
        public static Action<Collider> OnBossWeakPointHitted { get; set; }
        public static Action<bool> OnPlayerSneaking { get; set; }
    }
}

