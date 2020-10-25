using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        #region Fields

        private ButterflyData objData;
        private Transform objTransform;
        private Vector3 TargetPoint;

        private readonly float maxFlyAltitude;

        private float sittingTimer = 0.00f;
        private bool isSitting = false;

        #endregion


        #region Properties

        private Vector3 Position
        {
            get => objTransform.position;
            set => objTransform.position = value;
        }

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            objTransform = butterflyObject.transform;
            objTransform.localScale *= objData.Struct.Size;
            maxFlyAltitude = Position.y + objData.Struct.MaxFlyAltitudeFromSpawn;
        }

        #endregion


        #region Methods

        public void Act()
        {
            if (isSitting)
            {
                sittingTimer -= Time.deltaTime;
                if (sittingTimer <= 0) isSitting = false;
            }
            else
            {
                if (Position != TargetPoint)
                {
                    if (MaxAltitudeReached() && TargetPoint.y > Position.y)
                    {
                        Debug.Log(this + " maxFlyAltitude has reached");
                        ChangeTarget("Y"); 
                    }
                    TurnToTarget();
                    MoveToTarget();
                }
                else
                {
                    ChangeTarget();
                }
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            Debug.Log(this + " OnTriggerEnter(Collider collider)");

            if (collider.gameObject.tag == TagManager.GROUND)
            {
                ChangeTarget("Y");
                if (Random.Range(1, 100) > 50) SitDown();
            }
        }

        private void SitDown()
        {
            Debug.Log(this + " SitDown()");

            isSitting = true;
            sittingTimer = Random.Range(1.5f, 4f);

            Debug.Log(this+ " sittingTimer: " + sittingTimer);
        }

        private void ChangeTarget(string axis = null)
        {
            if (axis == null) TargetPoint = objData.NewTargetPoint(Position);
            else TargetPoint = objData.NewTargetPointInOppositeDirection(Position, TargetPoint - objTransform.position, axis);
        }

        private void TurnToTarget()
        {
            objTransform.rotation = objData.Turn(objTransform, TargetPoint);
        }

        private void MoveToTarget()
        {
            Position = objData.Move(Position, TargetPoint);
        }

        private bool MaxAltitudeReached()
        {
            return Position.y >= maxFlyAltitude;
        }

        #endregion
    }
}
