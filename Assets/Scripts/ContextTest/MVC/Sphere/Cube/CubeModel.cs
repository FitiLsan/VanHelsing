using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CubeModel
{
    #region Properties


    public BoxCollider BoxCollider { get; }
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
        BoxCollider = prefab.gameObject.GetComponent<BoxCollider>();
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        //SphereData.ChangeSphereCollider(SphereCollider, SphereStruct.SphereRadius);
        CubeData.Move(CubeTransform, CubeStruct.Target, CubeStruct.MoveSpeed);
    }

    #endregion
}
