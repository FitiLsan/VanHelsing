using UnityEngine;


[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Sphere", order = 0)]
public class SphereData : ScriptableObject
{
    #region Fields

    public float speed;

    public GameObject prefab;

    public GameObject Target;

    #endregion
}
