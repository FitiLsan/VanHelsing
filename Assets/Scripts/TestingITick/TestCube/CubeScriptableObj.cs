using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Cube", order = 0)]
public class CubeScriptableObj : ScriptableObject
{

    public float speed;

    public GameObject prefab;

    public GameObject Target;
    
}
