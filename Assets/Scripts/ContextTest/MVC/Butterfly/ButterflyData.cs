using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewButterfly", menuName = "CreateData/Butterfly", order = 0)]
public class ButterflyData : ScriptableObject
{
    #region Fields

    public ButterflyStruct Struct;

    #endregion


    #region Metods

    public Vector3 NewTargetPoint(Vector3 currentPosition)
    {
        float x = GetRandomCoord(currentPosition.x);
        float y = GetRandomCoord(currentPosition.y);
        float z = GetRandomCoord(currentPosition.z);
        return new Vector3(x, y, z);

        //return new Vector3(0, -10, 0); //for test
    }

    private float GetRandomCoord(float currentCoord)
    {
        return Random.Range(currentCoord - Struct.MaxDistanceFromCurrentPosition, currentCoord + Struct.MaxDistanceFromCurrentPosition);
    }

    public Vector3 Move(Vector3 currentPosition, Vector3 targetPoint)
    {
        return Vector3.MoveTowards(currentPosition, targetPoint, Struct.Speed);
    }

    //public Quaternion Turn(Quaternion current, Quaternion target)
    //{
    //    return Quaternion.RotateTowards(current, target, Struct.Speed);
    //}

    //public Vector3 Turn2(Vector3 current, Vector3 target)
    //{
    //    return Vector3.RotateTowards(current, target, Struct.Speed, 0.0f);
    //}

    //public Vector3 GetPointInDirection(Vector3 direction)
    //{

    //    return new Vector3();
    //}

    #endregion
}
