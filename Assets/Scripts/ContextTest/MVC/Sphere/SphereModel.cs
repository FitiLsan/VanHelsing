using UnityEngine;


public sealed class SphereModel
{
    #region Properties

    public SphereCollider SphereCollider { get; }
    public Transform SphereTransform { get; }
    public SphereData SphereData;
    public SphereStruct SphereStruct;

    #endregion


    #region ClassLifeCycle

    public SphereModel(GameObject prefab, SphereData spheredata)
    {
        SphereData = spheredata;
        SphereStruct = spheredata.SphereStruct;
        SphereTransform = prefab.transform;
        SphereCollider = prefab.gameObject.GetComponent<SphereCollider>();
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        SphereData.ChangeSphereCollider(SphereCollider, SphereStruct.SphereRadius);
        SphereData.Move(SphereTransform, SphereStruct.Target, SphereStruct.MoveSpeed);
    }

    #endregion
}
