using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class TrapBehaviour : MonoBehaviour
    {
        public TrapStruct TrapStruct;
        public int Type;
        public GameObject Projectile;
        private GameObject Curr;
        public Transform ProjectilePlace;
        private int Charge = 1;

        private void Awake()
        {
            if(transform.childCount != 0)
            {
                ProjectilePlace = transform.GetChild(0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(TagManager.PLAYER) && Type == 0)
            {
                if (Charge > 0)
                {
                    //TrapStruct Damage
                    Charge -= 1;
                }
                else
                {
                    Destroy(gameObject, 5);
                }
            }
            if (other.transform.CompareTag(TagManager.PLAYER) && Type == 1)
            {
                if(Charge > 0)
                {
                    Instantiate(Projectile, ProjectilePlace);
                    Charge -= 1;
                }
                else
                {
                    Destroy(gameObject, 5);
                }
            }
        }
    }
}
