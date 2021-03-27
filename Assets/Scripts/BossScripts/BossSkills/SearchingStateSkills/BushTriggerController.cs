using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Extensions;

namespace BeastHunter
{
    public class BushTriggerController : MonoBehaviour
    {
        public GameObject Seed;
        public GameObject Bush;
        private Rigidbody _rigidbody;
        private MeshRenderer _seedMesh;

        private void Awake()
        {
            _seedMesh = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            
        }

        private void Update()
        {
            
        }



        private void OnTriggerEnter(Collider other)
        {

            if (!Bush.activeSelf && other.CompareTag("Ground"))
            {
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                DOVirtual.DelayedCall(1f, GrowBush);
            }

            if (Bush.activeSelf)
            {
                var obj = other.GetComponent<InteractableObjectBehavior>();
                if (obj != null && !other.isTrigger)
                {
                    if (obj.Type == InteractableObjectType.Player || obj.Type == InteractableObjectType.Enemy)
                    {
                        var bushDamage = new Damage();
                        // bushDamage.PhysicalDamage = 75f;//Random.Range(33f, 50f);
                        //    Services.SharedInstance.AttackService.CountAndDealDamage(bushDamage, other.transform.GetMainParent().
                        //       gameObject.GetInstanceID());

                        var instanceID = other.transform.root.gameObject.GetInstanceID();
                        Services.SharedInstance.BuffService.AddTemporaryBuff(instanceID, Resources.Load("Data/Buffs/BushThornsDebuffData") as TemporaryBuff);
                    }

                }
            }
        }

        private void GrowBush()
        {
            _seedMesh.enabled = false;
            Bush.SetActive(true);
            transform.DOMoveZ(transform.position.z + 1f, 3);
            transform.DOScale(1.5f, 3f);
        }
    }
}