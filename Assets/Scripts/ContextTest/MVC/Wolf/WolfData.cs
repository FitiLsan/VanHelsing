﻿using UnityEngine;


[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Wolf", order = 0)]
public sealed class WolfData : ScriptableObject
{
    #region Fields

    public WolfStruct WolfStruct;

    #endregion


    #region Methods

    public void Move(Transform transform, Transform target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
    }

    public void ChangeWolfCollider(SphereCollider sphereCollider, float sphereRadius)
    {
        if(sphereCollider!=null)
        {
            sphereCollider.radius = sphereRadius;
        }
    }

    #endregion
}