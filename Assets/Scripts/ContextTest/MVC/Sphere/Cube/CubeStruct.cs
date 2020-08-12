using System.Collections;
using System;
using UnityEngine;


[Serializable]
public struct CubeStruct
{
    #region Fields
    
    public float MoveSpeed;
    
    public float BoxWidth;
    
    public GameObject Prefab;

    public Transform Target;

    #endregion
}
