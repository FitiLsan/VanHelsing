using BeastHunter;
using UnityEngine;

[CreateAssetMenu(fileName = "NewButterfly", menuName = "CreateData/Butterfly", order = 0)]
public class ButterflyData : ScriptableObject
{
    #region Fields

    private readonly float sin60 = Mathf.Sqrt(3) / 2;
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
    }

    #endregion


    #region Methods

    public void Act(ButterflyModel model)
    {
        Vector2 forward2D = new Vector2(model.ObjTransform.forward.x, model.ObjTransform.forward.z).normalized;
        Vector2 right2D = new Vector2(forward2D.y, -forward2D.x);

        Vector2 circlePoint2D = new Vector2(model.CirclePoint.x, model.CirclePoint.z);
        Vector2 butterflyPosition2D = new Vector2(model.Position.x, model.Position.z);

        Vector3 circlePointDirection = model.CirclePoint - model.Position;
        Vector2 circlePointDirection2D = circlePoint2D - butterflyPosition2D;

        Vector3 targetDirection = model.TargetPoint - model.Position;
        Vector2 targetDirection2D = new Vector2(model.TargetPoint.x, model.TargetPoint.z);

        if (model.IsCircling)
        {
            model.ObjTransform.RotateAround(model.CirclePoint, Vector3.up * model.RotateAroundDirection, 8/circlePointDirection2D.magnitude);    //скорость разворота вынести в структуру?

            Vector3 circlePointDirectionRotate = Quaternion.AngleAxis(-90 * model.RotateAroundDirection, Vector3.up) * circlePointDirection;
            Vector3 newDirection = Vector3.RotateTowards(model.ObjTransform.forward, circlePointDirectionRotate, Struct.TurnSpeed, 0.0f);
            newDirection.y = -sin60;
            model.ObjTransform.rotation = Quaternion.LookRotation(newDirection);

            float dot = Vector2.Dot(forward2D, targetDirection2D.normalized);
            if (dot > 0.99f) model.IsCircling = false;
        }
        else
        {
            if (model.IsSitting)
            {
                model.SittingTimer -= Time.deltaTime;
                if (model.SittingTimer <= 0)
                {
                    model.IsSitting = false;
                    model.IsCircling = false;
                }
            }
            else
            {
                if (model.Position != model.TargetPoint)
                {
                    if (model.Position.y >= model.MaxFlyAltitude && model.TargetPoint.y > model.Position.y)
                    {
                        Debug.Log(this + " maxFlyAltitude has reached");
                        model.TargetPoint = NewTargetPointInOppositeDirection(model.Position, model.TargetPoint - model.Position, "Y");
                        model.IsCircling = false;
                    }
                    model.ObjTransform.rotation = Turn(model.TargetPoint, model.Position, model.ObjTransform.forward);
                    model.Position = Move(model.TargetPoint, model.Position);
                }
                else
                {
                    model.TargetPoint = NewTargetPoint(model.Position);
                    model.CirclePoint = NewCirclePoint(model.Position);
                    model.IsCircling = true;

                    circlePoint2D = new Vector2(model.CirclePoint.x, model.CirclePoint.z);
                    circlePointDirection2D = circlePoint2D - butterflyPosition2D;
                    if (Vector2.Dot(right2D, circlePointDirection2D) > 0) model.RotateAroundDirection = 1;
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
            model.TargetPoint = NewTargetPointInOppositeDirection(model.Position, model.TargetPoint - model.Position, "Y");
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
        newDirection.y = -sin60;    //keep constant tilt of the butterfly
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
        float y = GetRandomCoordForCircle(0);                       //!!для теста
        float z = GetRandomCoordForCircle(currentPosition.z);
        return new Vector3(x, y, z);
    }

    private float GetRandomCoordForCircle(float coord, float? forwardCoord = null)
    {
        //float centerDistance = 3;
        //if (forwardCoord.HasValue && forwardCoord.Value != 0)
        //{
        //    if (forwardCoord > 0) return Random.Range(coord - centerDistance, coord - 1);
        //    else return Random.Range(coord + 1, coord + centerDistance);
        //}

        int sign = 1;
        if (Random.Range(1, 100) > 50) sign = -sign;
        float random = Random.Range(1, 2);
        return coord + random * sign;
    }

    #endregion
}
