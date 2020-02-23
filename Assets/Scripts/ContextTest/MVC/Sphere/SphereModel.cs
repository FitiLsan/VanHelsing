using UnityEngine;


public sealed class SphereModel
{
    #region Fields

    public float SphereSpeed;
    public GameObject SphereTarget { get; }
    public SphereCollider SphereRadius { get; }
    public Transform SphereTransform { get; }

    #endregion


    #region ClassLifeCycle

    public SphereModel(GameObject prefab, SphereData spheredata)
    {
        SphereSpeed = spheredata.speed;
        SphereTransform = prefab.transform;
        SphereTarget = spheredata.Target;
        SphereRadius = prefab.gameObject.GetComponent<SphereCollider>();
    }

    #endregion


    #region Metods

    public void Move()
    {
        SphereTransform.position = Vector3.MoveTowards
            (SphereTransform.position,
            SphereTarget.transform.position,
            SphereSpeed);
    }

    public void ChangeBox()
    {
        SphereRadius.radius = 0.5f;
    }

    #endregion
}
