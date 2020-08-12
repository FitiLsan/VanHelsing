using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "CreateData/Cube", order = 0)]
public sealed class CubeData : ScriptableObject
{
    #region Fields

    public CubeStruct CubeStruct;

    #endregion


    #region Metods

    public void Move(Transform transform, Transform target, float speed)
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            target.transform.position,
            speed);
    }

    public void ChangeBoxCollider(BoxCollider CubeCollider, float CubeX, float CubeY, float CubeZ)
    {
        if (CubeCollider != null)
        {
            CubeCollider.size = new Vector3(CubeX, CubeY, CubeZ);
        }
    }

    #endregion
}

