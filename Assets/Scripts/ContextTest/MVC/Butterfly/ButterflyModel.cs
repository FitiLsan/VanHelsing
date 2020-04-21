﻿using UnityEngine;


public sealed class ButterflyModel
{
    #region Fields

    public SphereCollider ButterflyCollider { get; }
    public Transform ButterflyTransform { get; }
    public ButterflyData ButterflyData;
    public ButterflyStruct ButterflyStruct;

    #endregion


    #region ClassLifeCycle

    public ButterflyModel(GameObject prefab, ButterflyData butterflydata)
    {
        ButterflyData = butterflydata;
        ButterflyStruct = butterflydata.ButterflyStruct;
        ButterflyTransform = prefab.transform;
        ButterflyCollider = prefab.gameObject.GetComponent<SphereCollider>();
    }

    #endregion


    #region Metods

    public void Initilize()
    {
        ButterflyData.Move(ButterflyTransform, ButterflyStruct.Target, ButterflyStruct.MoveSpeed);
    }

    #endregion
}