using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct WolfStruct
{
    #region Fields

    public GameObject Prefab;
    [Range(0.0f, 0.5f)]
    public float WalkSpeed;
    [Range(0.5f, 1.5f)]
    public float RunSpeed;
    public float CurrentSpeed;
    [Range(0.0f, 10.0f)]
    public float AggroRange;
    public GameObject CurrentTarget;
    public float MaxHealth;
    public float CurrentHealth;
    public Transform SpawnPosition;
    public GameObject PatrolWaypoints;
    public LayerMask TargetMask;
    public List<Vector3> PatrolWaypointsList;
    public List<GameObject> TargetsInAggroRange;

    #endregion
}
