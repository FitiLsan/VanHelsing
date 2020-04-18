using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GiantMudCrabProjectile
{
    #region Fields

    public float Damage = 0;
    public GameObject Mud;
    public Rigidbody RB;

    #endregion


    #region ClassLifeCycle

    public GiantMudCrabProjectile(float damage, Transform player, Transform CrabMouth, GameObject Prefab)
    {
        Damage = damage;
        Mud = GameObject.Instantiate(Prefab, CrabMouth.position, Quaternion.identity);
        RB = Mud.GetComponent<Rigidbody>();
        Vector3 forceVector = player.position - Mud.transform.position;
        RB.AddForce(forceVector * 100);
    }

    #endregion

}

