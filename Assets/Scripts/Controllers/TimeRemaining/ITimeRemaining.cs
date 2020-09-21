using System;


namespace BeastHunter
{
    public interface ITimeRemaining
    {
        bool IsActive { get; set; }
        Action Method { get; }
        bool IsRepeating { get; }
        float Time { get; set; }
        float CurrentTime { get; set; }
    }
}
