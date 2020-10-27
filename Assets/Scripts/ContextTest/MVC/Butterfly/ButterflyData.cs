using BeastHunter;
using UnityEngine;


[CreateAssetMenu(fileName = "NewButterfly", menuName = "CreateData/Butterfly", order = 0)]
public class ButterflyData : ScriptableObject
{
    #region Fields

    //private readonly float sin60 = Mathf.Sqrt(3) / 2;   //for constant tilt of the butterfly
    public ButterflyStruct Struct;

    #endregion


    #region ClassLifeCycle

    public ButterflyData()
    {
        Struct.Size = 0.5f;
        Struct.Speed = 0.1f;
        Struct.TurnSpeed = 0.03f;
        Struct.MaxFlyAltitudeFromSpawn = 5;
        Struct.MaxDistanceFromCurrentPosition = 10;
        Struct.CircularRotationSpeed = 6;
        Struct.MinCircleSize = 1;
        Struct.MaxCircleSize = 3;
    }

    #endregion


    #region Methods

    public void Act(ButterflyModel model)
    {
        Transform transform = model.ObjTransform;

        if (model.IsCircling)
        {
            float rotateAroundSpeed = Struct.CircularRotationSpeed / model.CirclePoint.ToVectorXZ().DirectionTo(transform.position.ToVectorXZ()).magnitude;
            transform.RotateAround(model.CirclePoint, Vector3.up * model.RotateAroundDirection, rotateAroundSpeed);
            transform.rotation = CirclingRotate(model.CirclePoint, transform.position, transform.forward, model.RotateAroundDirection);

            float dot = Vector2.Dot(transform.forward.ToVectorXZ().normalized, model.TargetPoint.ToVectorXZ().normalized);
            if (dot > 0.99f) model.IsCircling = false;
        }
        else
        {
            if (model.IsSitting)
            {
                model.SittingTimer -= Time.deltaTime;
                if (model.SittingTimer <= 0) model.IsSitting = false;
            }
            else
            {
                if (transform.position != model.TargetPoint)
                {
                    if (transform.position.y >= model.MaxFlyAltitude && model.TargetPoint.y > transform.position.y)
                    {
                        Debug.Log(this + " maxFlyAltitude has reached");
                        model.TargetPoint = NewTargetPointInOppositeDirection(transform.position, model.TargetPoint.DirectionTo(transform.position), "Y");
                    }
                    transform.rotation = Turn(model.TargetPoint, transform.position, transform.forward);
                    transform.position = Move(model.TargetPoint, transform.position);
                }
                else
                {
                    model.TargetPoint = NewTargetPoint(transform.position);
                    model.CirclePoint = NewCirclePoint(transform.position);
                    model.IsCircling = true;

                    Vector2 directionToCirclePoint = model.CirclePoint.ToVectorXZ().DirectionTo(transform.position.ToVectorXZ());
                    if (Vector2.Dot(transform.forward.ToVectorXZ().TurnToRight(), directionToCirclePoint) > 0) model.RotateAroundDirection = 1;
                    else model.RotateAroundDirection = -1;
                }
            }
        }
    }

    public void TriggerEnter(Collider collider, ButterflyModel model)
    {
        Debug.Log(this + " OnTriggerEnter(Collider collider)");

        if (collider.gameObject.tag == TagManager.GROUND)
        {
            model.TargetPoint = NewTargetPointInOppositeDirection(model.ObjTransform.position, model.TargetPoint - model.ObjTransform.position, "Y");
            if (Random.Range(1, 100) > 25)
            {
                Debug.Log(this + " is sitting");

                model.IsSitting = true;
                model.SittingTimer = Random.Range(1.5f, 4f);

                Debug.Log(this + " sittingTimer: " + model.SittingTimer);
            }
        }
    }

    private Vector3 Move(Vector3 targetPoint, Vector3 currentPosition)
    {
        return Vector3.MoveTowards(currentPosition, targetPoint, Struct.Speed);
    }

    private Quaternion Turn(Vector3 targetPoint, Vector3 position, Vector3 forward)
    {
        Vector3 targetDirection = targetPoint - position;
        Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, Struct.TurnSpeed, 0.0f);
        //newDirection.y = -sin60;    //keep constant tilt of the butterfly
        newDirection.y = forward.y;
        return Quaternion.LookRotation(newDirection);
    }

    private Quaternion CirclingRotate(Vector3 circlePoint, Vector3 position, Vector3 forward, int rotateAroundDirection)
    {
        Vector3 circlePointDirectionRotate90 = Quaternion.AngleAxis(-90 * rotateAroundDirection, Vector3.up) * circlePoint.DirectionTo(position);
        Vector3 newDirection = Vector3.RotateTowards(forward, circlePointDirectionRotate90, Struct.TurnSpeed, 0.0f);
        //newDirection.y = -sin60; //keep constant tilt of the butterfly
        newDirection.y = forward.y;
        return Quaternion.LookRotation(newDirection);
    }

    /// <summary>Return random coordinates within MaxDistanceFromCurrentPosition</summary>
    private Vector3 NewTargetPoint(Vector3 currentPosition)
    {
        float x = GetRandomCoord(currentPosition.x);
        float y = GetRandomCoord(currentPosition.y);
        float z = GetRandomCoord(currentPosition.z);
        return new Vector3(x, y, z);
    }

    /// <summary>Return random coordinates within MaxDistanceFromCurrentPosition in the opposite direction along the specified coordinate axis</summary>
    private Vector3 NewTargetPointInOppositeDirection(Vector3 currentPosition, Vector3 currentDirection, string axis)
    {
        Vector3 newTarget = NewTargetPoint(currentPosition);

        switch (axis.ToUpper())
        {
            case "X": newTarget.x = GetRandomCoord(currentPosition.x, currentDirection.x); break;
            case "Y": newTarget.y = GetRandomCoord(currentPosition.y, currentDirection.y); break;
            case "Z": newTarget.z = GetRandomCoord(currentPosition.z, currentDirection.z); break;
        }

        return newTarget;
    }

    private float GetRandomCoord(float currentCoord, float? directionCoord = null)
    {
        if (directionCoord.HasValue && directionCoord.Value != 0)
        {
            if (directionCoord > 0) return Random.Range(currentCoord - Struct.MaxDistanceFromCurrentPosition, currentCoord - 0.5f);
            else return Random.Range(currentCoord + 0.5f, currentCoord + Struct.MaxDistanceFromCurrentPosition);
        }
        return Random.Range(currentCoord - Struct.MaxDistanceFromCurrentPosition, currentCoord + Struct.MaxDistanceFromCurrentPosition); ;
    }

    private Vector3 NewCirclePoint(Vector3 currentPosition)
    {
        float x = GetRandomCoordForCircle(currentPosition.x);
        float z = GetRandomCoordForCircle(currentPosition.z);
        return new Vector3(x, currentPosition.y, z);
    }

    private float GetRandomCoordForCircle(float coord)
    {
        int sign = 1;
        if (Random.Range(1, 100) > 50) sign = -sign;
        float random = Random.Range(Struct.MinCircleSize, Struct.MaxCircleSize);
        return coord + random * sign;
    }

    #endregion
}



static class VectorExtension
{
    /// <summary>Convert Vector3 into Vector2 for XZ-plane</summary>
    public static Vector2 ToVectorXZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);

    /// <summary>Rotates the vector2 90 degrees</summary>
    public static Vector2 TurnToRight(this Vector2 vector2) => new Vector2(vector2.y, -vector2.x);

    /// <summary>Returns the direction vector2 to the specified point</summary>
    public static Vector2 DirectionTo(this Vector2 vector2, Vector2 target) => vector2 - target;

    /// <summary>Returns the direction vector3 to the specified point</summary>
    public static Vector3 DirectionTo(this Vector3 vector3, Vector3 target) => vector3 - target;
}