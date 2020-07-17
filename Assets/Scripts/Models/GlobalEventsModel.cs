using System;


namespace BeastHunter
{
    public static class GlobalEventsModel
    {
        public static Action OnPlayerDie { get; set; }
        public static Action OnBossStunned { get; set; }
    }
}

