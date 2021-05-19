using System;

namespace BeastHunter
{
    [Flags]
    public enum SpawnerTriggerType
    {
        None    = 0,
        OneTime = 1 << 0,
        Timer   = 1 << 1
        //etc.
    }
}