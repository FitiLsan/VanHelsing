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
        private bool _isLoad;
        private bool _isLoading;
        private bool _isShooting;

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
                VerticalTurn.rotation *= Quaternion.Euler(50 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                VerticalTurn.rotation *= Quaternion.Euler(-50 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.A))
            {
                HorizontalTurn.rotation *= Quaternion.Euler(-37.5f * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                HorizontalTurn.rotation *= Quaternion.Euler(37.5f * Time.deltaTime, 0 , 0);
            }
        }

        private void Reload()
        {
            if(_isLoad || _isShooting || _isLoading)
            {
                return;
            }
            _isLoading = true;
            _animator.SetTrigger("StartLoad");
            DG.Tweening.DOVirtual.DelayedCall(2.5f, OnLoading);
            void OnLoading()
            {
                _isLoad = true;
                _isLoading = false;
                _animator.SetBool("isLoad", _isLoad);
            }
        }    
        private void Shoot()
        {
            if (!_isLoad || _isLoading || _isShooting)
            {
                return;
            }
            _isShooting = true;
            _isLoad = false;
            _animator.SetTrigger("Shoot");
            BoltRb.isKinematic = false;
            BoltRb.AddForce(BoltRb.transform.up * -1 * 100f, ForceMode.Impulse);
            BoltRb.transform.SetParent(null);
            DG.Tweening.DOVirtual.DelayedCall(1f, () => _animator.SetBool("isLoad", _isShooting = false));
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