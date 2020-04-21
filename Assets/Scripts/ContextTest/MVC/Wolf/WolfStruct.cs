using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct WolfStruct
{
    #region Fields

    [Range(0.0f, 1.0f)]
    public float WalkSpeed;
    [Range(1.25f, 3.0f)]
    public float RunSpeed;
    public float CurrentSpeed;
    [Range(0.0f, 10.0f)]
    public float AggroRange;
    public float MaxHealth;
    public float CurrentHealth;
    public bool IsPatroling;
    public Transform SpawnPosition;
    public GameObject Prefab;
    public GameObject PatrolWaypoints;
    public List<Vector3> PatrolWaypointsList;
    public List<GameObject> TargetsInAggroRange;

    #endregion
}
