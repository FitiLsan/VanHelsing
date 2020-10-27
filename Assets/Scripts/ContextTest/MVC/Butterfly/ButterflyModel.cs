using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        #region Fields

        private ButterflyData objData;
        public Transform ObjTransform;

        public Vector3 TargetPoint;
        public readonly float MaxFlyAltitude;

        public Vector3 CirclePoint;
        public int RotateAroundDirection;
        public bool IsCircling = false;

        public float SittingTimer = 0.00f;
        public bool IsSitting = false;

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            ObjTransform = butterflyObject.transform;
            ObjTransform.localScale *= objData.Struct.Size;
            MaxFlyAltitude = ObjTransform.position.y + objData.Struct.MaxFlyAltitudeFromSpawn;
        }

        #endregion


        #region Methods

        public void Execute() => objData.Act(this);
        public void OnTriggerEnter(Collider collider) => objData.TriggerEnter(collider, this);

        #endregion
    }
}
