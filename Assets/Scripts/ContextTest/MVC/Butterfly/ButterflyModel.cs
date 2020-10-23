using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        public Vector3 TargetPoint;
        ButterflyData objData;
        Transform objTransform;

        Vector3 Position
        {
            get
            {
                return objTransform.position;
            }
            set
            {
                objTransform.position = value;
            }
        }

        Vector3 Direction
        {
            get
            {
                return objTransform.forward;
            }
            set
            {
                objTransform.forward = value;
            }
        }

        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            objTransform = butterflyObject.transform;
        }

        #endregion


        #region Metods

        public void Act()
        {
            if (Position != TargetPoint)
            {
                Vector3 targetDirection = TargetPoint - Position;
                Vector3 newDirection = Vector3.RotateTowards(Direction, targetDirection, 0.1f, 0.0f);
                //Vector3 newDirection = Vector3.RotateTowards(new Vector3(Direction.x, 0 ,Direction.z), new Vector3(targetDirection.x, 0, targetDirection.z), 0.1f, 0.0f);
                //Vector3 newDirection = Vector3.RotateTowards(new Vector3(0, Direction.y, 0), new Vector3(0, targetDirection.y, 0), 0.1f, 0.0f);
                objTransform.rotation = Quaternion.LookRotation(newDirection);

                //objTransform.transform.rotation = objData.Turn(objTransform.transform.rotation, Quaternion.FromToRotation(Vector3.forward, Vector3.right));
                MoveToTarget();
            }
            else
            {
                ChangeTarget();
            }
        }

        #endregion


        void MoveToTarget()
        {
            Position = objData.Move(Position, TargetPoint);
        }

        public void ChangeTarget()
        {
            Debug.Log("Butterfly.ChangeTarget()");
            TargetPoint = objData.NewTargetPoint(Position);
        }
    }
}
