using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTrigger : MonoBehaviour
{
    public static event Action<GameObject,GameObject> CatchedEvent;
    public static event Action ThrowEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Cylinder"))
        {
            CatchedEvent?.Invoke(gameObject, other.gameObject);
            //other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Cylinder"))
        {
            ThrowEvent?.Invoke();
        }
    }
}
