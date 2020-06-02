using UnityEngine;
using System;


[Serializable]
public struct ButterFlyStruct
{
    #region Fields
    public float MoveSpeed;

    public float ButterFlyRadius;

    public bool NPC;

    //Events

    public bool Sleep;

    public bool Aggressive;

    public int Health;

    public GameObject Prefab;

    public Transform Target;
    #endregion

}
