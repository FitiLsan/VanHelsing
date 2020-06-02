using UnityEngine;


public sealed class ButterFlyModel
{
    #region Properties

    public SphereCollider ButterFlyCollider;
    public Transform ButterFlyTransform;
    public ButterFlyData ButterFlyData;
    public ButterFlyStruct ButterFlyStruct;

    #endregion


    #region ClassLifeCycle

    public ButterFlyModel(GameObject prefab, ButterFlyData butterflydata)
    {
        ButterFlyData = butterflydata;
        ButterFlyStruct = butterflydata.ButterFlyStruct;
        ButterFlyTransform = prefab.transform;
        ButterFlyCollider = prefab.gameObject.GetComponent<SphereCollider>();
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        ButterFlyData.ChangeSphereCollider(ButterFlyCollider, ButterFlyStruct.ButterFlyRadius);
        ButterFlyData.Move(ButterFlyTransform, ButterFlyStruct.Target, ButterFlyStruct.MoveSpeed);
    }

    #endregion
}
