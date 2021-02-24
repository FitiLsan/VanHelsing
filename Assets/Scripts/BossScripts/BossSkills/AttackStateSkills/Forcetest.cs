using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class Forcetest : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                var force = 30f;
                var list = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(transform.position, 20f, "Player");
                if (list.Count != 0)
                {
                   var go = list.Find(x => x.name == "Player");
                  var pm =  go.transform.parent.Find("PuppetMaster").GetComponent<PuppetMaster>();
                    var vec = go.transform.position - transform.position;
                    pm.state = PuppetMaster.State.Dead;
                    go.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, 10f, 1f, ForceMode.Impulse);


                    TimeRemaining timeRemaining = new TimeRemaining(() => pm.state = PuppetMaster.State.Alive, 2f);
                    timeRemaining.AddTimeRemaining();
                    
                    
                    
                    Debug.Log("boom");
                }
            }
        }
    }
}