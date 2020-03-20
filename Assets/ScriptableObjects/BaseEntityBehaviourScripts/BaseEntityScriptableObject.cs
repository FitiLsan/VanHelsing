using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseEntityScriptableObject : ScriptableObject
{
    public abstract void EntityBehaviour(Transform transform);
}
