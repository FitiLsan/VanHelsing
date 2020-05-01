using System;
using Common;

namespace Events.Args
{
    public class DayTimeArgs : EventArgs
    {
        public DayTimeArgs(DayTimeTypes dayTime)
        {
            DayTime = dayTime;
        }

        private DayTimeTypes DayTime { get; }
    }
}