using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewButterfly", menuName = "CreateData/Butterfly", order = 0)]
public class ButterflyData : ScriptableObject
{
    #region Fields

    private readonly float sin60 = Mathf.Sqrt(3) / 2;
    public ButterflyStruct Struct;

    #endregion


    #region Metods

    public Vector3 NewTargetPoint(Vector3 currentPosition)
    {
        float x = GetRandomCoord(currentPosition.x);
        float y = GetRandomCoord(currentPosition.y);
        float z = GetRandomCoord(currentPosition.z);
        return new Vector3(x, y, z);
    }

    private float GetRandomCoord(float currentCoord)
    {
        return Random.Range(currentCoord - Struct.MaxDistanceFromCurrentPosition, currentCoord + Struct.MaxDistanceFromCurrentPosition);
    }

    public Vector3 Move(Vector3 currentPosition, Vector3 targetPoint)
    {
        return Vector3.MoveTowards(currentPosition, targetPoint, Struct.Speed);
    }

    public Quaternion Turn(Transform transform, Vector3 targetPoint)
    {
        Vector3 targetDirection = targetPoint - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Struct.TurnSpeed, 0.0f);
        return Quaternion.LookRotation(new Vector3(newDirection.x, -sin60, newDirection.z));
    }

    #endregion
}
