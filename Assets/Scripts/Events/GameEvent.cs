using System;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    ///     Базовый класс для событий
    /// </summary>
    public class GameEvent : UnityEvent<EventArgs>
    {
    }
}