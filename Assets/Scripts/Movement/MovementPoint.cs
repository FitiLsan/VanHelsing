using UnityEngine;

namespace BeastHunter
{
    public sealed class MovementPoint
    {
        #region Properties

        public Vector3 Position { get; set; }
        public bool IsGrounded { get; set; }
        public float WaitingTime { get; set; }
        public string AnimationState { get; set; }

        #endregion
    }
}