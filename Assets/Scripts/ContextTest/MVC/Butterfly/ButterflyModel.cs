using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        private Vector3 TargetPoint;
        private ButterflyData objData;
        private Transform objTransform;
        private readonly float maxFlyAltitude;

        private Vector3 Position
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

        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            objTransform = butterflyObject.transform;
            objTransform.localScale *= objData.Struct.Size;
            maxFlyAltitude = Position.y + objData.Struct.MaxFlyAltitudeFromSpawn;
        }

        #endregion


        #region Metods

        public void Act()
        {
            if (Position != TargetPoint)
            {
                if (MaxAltitudeReached())
                {
                    Debug.Log(this + " MaxAltitudeReached() " + maxFlyAltitude);
                    //ChangeTarget();
                    TargetPoint = -TargetPoint / 2;
                    Debug.Log(this + " Next point " + TargetPoint);
                }
                    TurnToTarget();
                    MoveToTarget();
            }
            else
            {
                ChangeTarget();
            }
        }

        public void OnTriggerEnter(ITrigger trigger, Collider collider)
        {
            TargetPoint = -TargetPoint;
        }

        private void ChangeTarget()
        {
            //Debug.Log("Butterfly.ChangeTarget()");
            TargetPoint = objData.NewTargetPoint(Position);
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
