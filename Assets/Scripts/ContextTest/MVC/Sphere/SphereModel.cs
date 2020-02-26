using UnityEngine;


public sealed class SphereModel
{
    #region Fields

    public float SphereSpeed;
    public float SphereRadius;
    public GameObject SphereTarget { get; }
    public SphereCollider SphereCollider { get; }
    public Transform SphereTransform { get; }

    #endregion


    #region ClassLifeCycle

    public SphereModel(GameObject prefab, SphereData spheredata)
    {
        SphereSpeed = spheredata.MoveSpeed;
        SphereTransform = prefab.transform;
        SphereTarget = spheredata.Target;
        SphereCollider = prefab.gameObject.GetComponent<SphereCollider>();
        SphereRadius = spheredata.SphereRadius;
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        //SphereData.Execute(SphereTransform, SphereTarget);
    }

    public void Move()
    {
        SphereTransform.position = Vector3.MoveTowards
            (SphereTransform.position,
            SphereTarget.transform.position,
            SphereSpeed);
    }

    public void ChangeBox()
    {
        SphereCollider.radius = SphereRadius;
    }

    #endregion
}
