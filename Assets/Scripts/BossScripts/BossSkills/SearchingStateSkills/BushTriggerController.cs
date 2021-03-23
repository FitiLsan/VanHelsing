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
                if (obj!=null && obj.Type == InteractableObjectType.Player && !other.isTrigger)
                {
                    var bushDamage = new Damage();
                    bushDamage.PhysicalDamage = Random.Range(3f, 10f);
                    Services.SharedInstance.AttackService.CountAndDealDamage(bushDamage, other.transform.GetMainParent().
                        gameObject.GetInstanceID());
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