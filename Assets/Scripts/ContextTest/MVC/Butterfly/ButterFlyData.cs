using UnityEngine;


[CreateAssetMenu(fileName = "NewData", menuName = "CreateData/ButterFly", order = 1)]
public sealed class ButterFlyData : ScriptableObject
{
    #region Fields

    public ButterFlyStruct ButterFlyStruct;

    #endregion

    #region Metods

    public void Move(Transform transform, Transform target, float speed)
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            target.transform.position,
            speed);
    }

    public void ChangeSphereCollider(SphereCollider ButterFlyCollider, float ButterFlyRadius)
    {
        if (ButterFlyCollider != null)
        {
            ButterFlyCollider.radius = ButterFlyRadius;
        }
    }

    #endregion
}
