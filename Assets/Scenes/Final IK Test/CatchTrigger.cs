using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchTrigger : MonoBehaviour
{
    public static event Action<GameObject,GameObject> CatchedEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            CatchedEvent?.Invoke(gameObject, other.gameObject);
        }
    }
}
