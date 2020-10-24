using UnityEngine;
using System;
using UnityEngine.Internal;

[Serializable]
public struct ButterflyStruct
{
    #region Fields

    public GameObject Prefab;

    [Tooltip("Default: 0.5")]
    public float Size;

    [Tooltip("Default: 10")]
    public float MaxDistanceFromCurrentPosition;

    [Tooltip("Default: 0.1")]
    public float Speed;

    [Tooltip("Default: 0.03")]
    public float TurnSpeed;

    [Tooltip("Default: 5")]
    public float MaxFlyAltitudeFromSpawn;

    #endregion
}

