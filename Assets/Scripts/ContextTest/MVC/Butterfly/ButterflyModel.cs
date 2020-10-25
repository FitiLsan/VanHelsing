//using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        #region Fields

        private Vector3 TargetPoint;
        private ButterflyData objData;
        private Transform objTransform;

        private readonly float maxFlyAltitude;

        private float sittingTimer;
        private bool isSitting = false;

        //GameObject targetPoint; //for debug

        #endregion


        #region Properties

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

        private Vector3 Direction
        {
            get
            {
                return TargetPoint - objTransform.position;
            }
        }

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            objData = butterflyData;
            objTransform = butterflyObject.transform;
            objTransform.localScale *= objData.Struct.Size;
            maxFlyAltitude = Position.y + objData.Struct.MaxFlyAltitudeFromSpawn;

            //targetPoint = new GameObject("TargetPoint"); //for debug
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
                    if (MaxAltitudeReached() && TargetPoint.y > Position.y) ChangeTarget("Y");
                    TurnToTarget();
                    MoveToTarget();
                }
                else
                {
                    ChangeTarget();
                }
            }
        }

        public void OnTriggerEnter(ITrigger trigger, Collider collider)
        {
            if (collider.gameObject.tag == TagManager.GROUND)
            {
                ChangeTarget("Y");
                if (Random.Range(1, 100) > 50) SitDown();
            }
        }

        private void SitDown()
        {
            isSitting = true;
            sittingTimer = Random.Range(1.5f, 4f);

            Debug.Log(this+ " sittingTimer: " + sittingTimer);
        }

        private void ChangeTarget(string axis = null)
        {
            if (axis == null) TargetPoint = objData.NewTargetPoint(Position);
            else TargetPoint = objData.NewTargetPointInOppositeDirection(Position, Direction, axis);

            //targetPoint.transform.position = TargetPoint; //for debug
            //Debug.Log(this + " new TargetPoint " + TargetPoint);
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
            //if (Position.y >= maxFlyAltitude) //for debug
            //{
            //    Debug.Log(this + " MaxAltitudeReached(): " + maxFlyAltitude);
            //    Debug.Log(this + " current position " + Position);
            //}
            return Position.y >= maxFlyAltitude;
        }

        #endregion
    }
}
