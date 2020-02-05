using UnityEngine;

public class SphereModel
{


    #region Property

    public GameObject SphereTarget;

    public SphereCollider SphereRadius;

    public Transform SphereTransform;

    public float SphereSpeed;

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

}
