using System;
using UnityEngine;

namespace Models
{
    [Obsolete("Не рекомендуется")]
    public class PCInput
    {
        public enum MouseButtons
        {
            Left = 0,
            Right = 1,
            Center = 2
        }

        public KeyCode Sprint { get; } = KeyCode.LeftShift;

        public KeyCode DefenceButton { get; } = KeyCode.LeftAlt;

        public KeyCode Jump { get; } = KeyCode.Space;

        public KeyCode Roll { get; } = KeyCode.LeftControl;

        public MouseButtons AimMouseButton { get; } = MouseButtons.Right;

        public MouseButtons LeftMouseButton { get; } = MouseButtons.Left;

        public MouseButtons AlternativeFire { get; } = MouseButtons.Center;

        public KeyCode Crouch { get; } = KeyCode.C;

        public KeyCode Inventory { get; } = KeyCode.I;

        public KeyCode ActionButton { get; } = KeyCode.F;

        public KeyCode TargetLock { get; } = KeyCode.T;

        public KeyCode CameraCenter { get; } = KeyCode.H;
    }
}