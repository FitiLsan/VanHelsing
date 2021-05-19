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

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _projectileData.DestroyProjectileAfterInstantiation(this as IProjectile);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_projectileData.FilterCollision(collision))
            {
                _projectileData.HitProjectile(this as IProjectile, collision);
            }           
        }

        #endregion
    }
}

