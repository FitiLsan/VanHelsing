using UnityEngine;


[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Sphere", order = 0)]
public class SphereData : ScriptableObject
{
    #region Fields

    public float MoveSpeed;

    public float SphereRadius;

    public GameObject Prefab;

    public GameObject Target;

    #endregion

    public void Execute(Transform transform, Transform target)
    {
        //Move()
    }
    
}
