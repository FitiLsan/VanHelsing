using BeastHunter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDo : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            int rnd = Random.Range(1, 100);
            if (rnd > 90)
            {
                target.GetComponent<InteractableObjectBehavior>().DoSmthEvent("succsessfully " + rnd);
            }
        }
        else
        {
            target = GameObject.FindGameObjectsWithTag("NPC")[0];
        }
    }
}
