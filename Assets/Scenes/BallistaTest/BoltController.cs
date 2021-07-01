using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class BoltController : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        private FixedJoint _fixedJoint;
        public bool _isActive;
        public float _velocityMult;
        public float _angularVelocityMult;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
        private void FixedUpdate()
        {
            Vector3 cross = Vector3.Cross(transform.forward, _rigidbody.velocity.normalized);
            _rigidbody.AddTorque(cross * _rigidbody.velocity.magnitude * _velocityMult);
            _rigidbody.AddTorque((-_rigidbody.angularVelocity + Vector3.Project(_rigidbody.angularVelocity, transform.forward) * _rigidbody.velocity.magnitude * _angularVelocityMult));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_isActive)
            {
                return;
            }
            var targetPoint = Instantiate(new GameObject());
            targetPoint.transform.SetParent(collision.transform);

            transform.SetParent(targetPoint.transform);

            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }
    }
}