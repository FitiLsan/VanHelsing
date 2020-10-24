using UnityEngine;
using System;
using UnityEngine.Internal;

[Serializable]
public struct ButterflyStruct
{
    #region Fields

    public GameObject Prefab;

    /// <summary>butterfly size</summary>
    [Tooltip("Default: 0.5")]
    public float Size;

    /// <summary>maximum distance of the coordinate from the current position</summary>
    [Tooltip("Default: 10")]
    public float MaxDistanceFromCurrentPosition;

    /// <summary>butterfly fly speed</summary>
    [Tooltip("Default: 0.1")]
    public float Speed;

    /// <summary>butterfly turn speed</summary>
    [Tooltip("Default: 0.03")]
    public float TurnSpeed;

    /// <summary>maximum butterfly flight height from the spawn point</summary>
    [Tooltip("Default: 5")]
    public float MaxFlyAltitudeFromSpawn;

    #endregion
}

