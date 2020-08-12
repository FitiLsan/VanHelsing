using UnityEngine;
using System;


[Serializable]
public struct CubeStruct
{
    #region Fields

    public float MoveSpeed;

    public float CubeX;

    public float CubeY;

    public float CubeZ;

    public GameObject Prefab;

    public Transform Target;

    #endregion
}