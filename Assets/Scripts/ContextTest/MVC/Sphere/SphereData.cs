using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Sphere", order = 0)]
public class SphereData : ScriptableObject
{
    public float speed;

    public GameObject prefab;

    public GameObject Target;
}
