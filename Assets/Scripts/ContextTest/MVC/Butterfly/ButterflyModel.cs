using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyModel
    {
        Vector3 targetPoint;
        ButterflyStruct butterflyStruct;
        ButterflyData butterflyData;
        GameObject butterfly; //потом оставить только конкретные компоненты

        Vector3 Position
        {
            get
            {
                return butterfly.transform.position;
            }
            set
            {
                butterfly.transform.position = value;
            }
        }

        #region ClassLifeCycle

        public ButterflyModel(GameObject butterflyObject, ButterflyData butterflyData)
        {
            butterfly = butterflyObject;
            this.butterflyData = butterflyData;
            butterflyStruct = butterflyData.ButterflyStruct;
        }

        #endregion


        #region Metods

        public void Act()
        {
            if (Position != targetPoint)
            {
                Position = Vector3.MoveTowards(Position, targetPoint, butterflyStruct.Speed);
            }
            else
            {
                targetPoint = NextTargetPoint();
            }
        }

        #endregion

        void MoveTo(Vector3 point)
        {

        }

        Vector3 NextTargetPoint()
        {
            float x = GetRandomCoord(Position.x);
            float y = GetRandomCoord(Position.y);
            float z = GetRandomCoord(Position.z);
            return new Vector3(x,y,z);
        }

        float GetRandomCoord(float currentCoord)
        {
            return Random.Range(currentCoord - butterflyStruct.MaxDistanceFromCurrentPosition, currentCoord + butterflyStruct.MaxDistanceFromCurrentPosition);
        }
    }
}
