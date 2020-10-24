using UnityEngine;


[CreateAssetMenu(fileName = "NewData", menuName = "CreateData/Sphere", order = 0)]
public sealed class SphereData : ScriptableObject
{
    #region Fields

    public SphereStruct SphereStruct;

    #endregion


    #region Metods

    public void Move(Transform transform, Transform target, float speed)
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            target.transform.position,
            speed);
    }

    public void ChangeSphereCollider(SphereCollider SphereCollider, float SphereRadius)
    {
        if(SphereCollider != null)
        {
            SphereCollider.radius = SphereRadius;
        }       
    }

    #endregion
}
