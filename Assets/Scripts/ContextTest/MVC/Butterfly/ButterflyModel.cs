using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        #region Fields

        private ButterflyData objData;
        private Transform objTransform;

        public float SittingTimer = 0.00f;
        public bool IsSitting = false;

        public Vector3 TargetPoint;
        public readonly float MaxFlyAltitude;

        #endregion


        #region Properties

        public Vector3 Position
        {
            get => objTransform.position;
            set => objTransform.position = value;
        }

        public Quaternion Rotation
        {
            get => objTransform.rotation;
            set => objTransform.rotation = value;
        }

        public Vector3 Forward
        {
            get => objTransform.forward;
        }

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            objTransform = butterflyObject.transform;
            objTransform.localScale *= objData.Struct.Size;
            MaxFlyAltitude = Position.y + objData.Struct.MaxFlyAltitudeFromSpawn;
        }

        #endregion


        #region Methods

        public void Execute() => objData.Act(this);
        public void OnTriggerEnter(Collider collider) => objData.TriggerEnter(collider, this);

        #endregion
    }
}
