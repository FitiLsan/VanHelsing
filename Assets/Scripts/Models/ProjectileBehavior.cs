using UnityEngine;


namespace BeastHunter
{
    public sealed class ProjectileBehavior : MonoBehaviour, IProjectile
    {
        #region Fields

        [SerializeField] private ProjectileData _projectileData;

        #endregion


        #region Properties

        public GameObject GameObject => gameObject;
        public ProjectileData ProjectileData => _projectileData;
        private Rigidbody _rigidbody;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            
            _projectileData.DestroyProjectileAfterInstantiation(this as IProjectile);
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_projectileData.FilterCollision(collision))
            {
                _projectileData.HitProjectile(this as IProjectile, collision);
            }
        }

        private void FixedUpdate()
        {
            
            Vector3 cross = Vector3.Cross(transform.forward, _rigidbody.velocity.normalized);
            _rigidbody.AddTorque(cross * _rigidbody.velocity.magnitude);// * _velocityMult);
            _rigidbody.AddTorque((-_rigidbody.angularVelocity + Vector3.Project(_rigidbody.angularVelocity, transform.forward) * _rigidbody.velocity.magnitude));// * _angularVelocityMult));
        }

        #endregion
    }
}

