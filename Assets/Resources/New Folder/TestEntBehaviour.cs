using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class TestEntBehaviour : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rigidbody;
        private RigidbodyConstraints _rigidbodyDefaultConstraints;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbodyDefaultConstraints = _rigidbody.constraints;
        }
        public void StateFall()
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.isKinematic = true;
        }
        public void StateStand()
        {
            _rigidbody.constraints = _rigidbodyDefaultConstraints;
            _rigidbody.isKinematic = false;
        }
    }
}