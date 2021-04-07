using UnityEngine;


namespace BeastHunter
{
    public sealed class Drag
    {
        #region Properties

        public Vector3 DragPointPosition { get; }
        public DragType Type { get; }
        public float HearingDistance { get; }

        #endregion


        #region ClassLifeCycle

        public Drag(Vector3 dragPointPosition, DragType type, float hearingDistance)
        {
            DragPointPosition = dragPointPosition;
            Type = type;
            HearingDistance = hearingDistance;
        }

        #endregion
    }
}

