using UnityEngine;

public sealed class CubeModel
{
    #region Properties

    public BoxCollider CubeCollider { get; }
    public Transform CubeTransform { get; }
    public CubeData CubeData;
    public CubeStruct CubeStruct;

    #endregion


    #region ClassLifeCycle

    public CubeModel(GameObject prefab, CubeData cubedata)
    {
        CubeData = cubedata;
        CubeStruct = cubedata.CubeStruct;
        CubeTransform = prefab.transform;
        CubeCollider = prefab.gameObject.GetComponent<BoxCollider>();
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        CubeData.ChangeBoxCollider(CubeCollider, CubeStruct.CubeX, CubeStruct.CubeY, CubeStruct.CubeZ);
        CubeData.Move(CubeTransform, CubeStruct.Target, CubeStruct.MoveSpeed);
    }

    #endregion
}
