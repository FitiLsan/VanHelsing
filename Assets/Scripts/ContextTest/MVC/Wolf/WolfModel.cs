using UnityEngine;


public sealed class WolfModel
{
    #region Properties

    public SphereCollider WolfCollider { get; }
    public Transform WolfTransfrom { get; }
    public WolfData WolfData;
    public WolfStruct WolfStruct;

    #endregion


    #region ClassLifeCycle

     public WolfModel(GameObject prefab, WolfData wolfData)
    {
        WolfData = wolfData;
        WolfStruct = wolfData.WolfStruct;
        WolfTransfrom = prefab.transform;
        WolfCollider = prefab.transform.GetComponent<SphereCollider>();
    }

    #endregion


    #region Methods

    public void Initialize()
    {
        WolfData.Move(WolfTransfrom, WolfStruct.Target, WolfStruct.MoveSpeed);
    }

    #endregion

}
