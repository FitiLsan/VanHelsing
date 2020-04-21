using UnityEngine;


[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Butterfly", order = 0)]
public sealed class ButterflyData : ScriptableObject
{
    #region Fields

    public ButterflyStruct ButterflyStruct;

    #endregion


    #region Metods

    public void Move(Transform transform, Transform target, float speed)
    {
        transform.position = Vector3.MoveTowards
            (transform.position,
            target.transform.position,
            speed);
    }

    #endregion
}
