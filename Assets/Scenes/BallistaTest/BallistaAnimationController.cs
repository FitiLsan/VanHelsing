using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class BallistaAnimationController : MonoBehaviour
    {
        private Animator _animator;
        public Rigidbody BoltRb;
        public GameObject Bolt;
        public Transform BoltPointTransform;
        public Transform HorizontalTurn;
        public Transform VerticalTurn;
        public bool IsActive;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if(!IsActive)
            {
                return;
            }    

            if (Input.GetKeyDown(KeyCode.C))
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                Shoot();
            }
            if(Input.GetKey(KeyCode.W))
            {
                VerticalTurn.rotation *= Quaternion.Euler(100 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                VerticalTurn.rotation *= Quaternion.Euler(-100 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                HorizontalTurn.rotation *= Quaternion.Euler(100 * Time.deltaTime,0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                HorizontalTurn.rotation *= Quaternion.Euler(-100 * Time.deltaTime,0 , 0);
            }
        }

        private void Reload()
        {
            _animator.SetTrigger("StartLoad");
            DG.Tweening.DOVirtual.DelayedCall(2.5f, () => _animator.SetBool("isLoad", true));
        }    
        private void Shoot()
        {
            _animator.SetTrigger("Shoot");
            BoltRb.isKinematic = false;
            BoltRb.AddForce(BoltRb.transform.up * -1 * 100f, ForceMode.Impulse);
            BoltRb.transform.SetParent(null);
            DG.Tweening.DOVirtual.DelayedCall(1f, () => _animator.SetBool("isLoad", false));
            DG.Tweening.DOVirtual.DelayedCall(0.5f, () => SetNewBolt());
        }

        private void SetNewBolt()
        {
            var newBolt = Instantiate(Bolt, BoltPointTransform.position, BoltPointTransform.rotation, BoltPointTransform);
            BoltRb = newBolt.GetComponent<Rigidbody>();
            BoltRb.isKinematic = true;
        }
    }
}