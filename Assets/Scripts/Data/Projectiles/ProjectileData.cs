using UnityEngine;
using Extensions;


namespace BeastHunter
{
    public abstract class ProjectileData : ScriptableObject
    {
        #region Fields

        [SerializeField] private ProjectileTypeEnum _projectileType;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Sound _flyingSound;
        [SerializeField] private Sound _collisionSound;
        [SerializeField] private Damage _projectileDamage;

        [SerializeField] private bool _isDestroyedAfterInstantiation;
        [SerializeField] private bool _isDestroyedAfterHit;
        [SerializeField] private float _timeToDestroyAfterInstantiation;
        [SerializeField] private float _timeToDestroyAfterHit;
        [SerializeField] private float _projectileColliderSize;

        #endregion


        #region Properties

        public ProjectileTypeEnum ProjectileType => _projectileType;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public Sound FlyingSound => _flyingSound;
        public Sound CollisionSound => _collisionSound;
        public Damage ProjectileDamage => _projectileDamage;

        protected GameContext Context { get; private set; }

        public bool IsDestroyedAfterInstantiation => _isDestroyedAfterInstantiation;
        public bool IsDestroyedAfterHit => _isDestroyedAfterHit;
        public float TimeToDestroyAfterInstantiation => _timeToDestroyAfterInstantiation;
        public float TImeToDestroyAfterHit => _timeToDestroyAfterHit;
        public float ProjectileColliderSize => _projectileColliderSize;

        #endregion


        #region Methods

        public virtual void Launch(GameContext context, Vector3 position, Vector3 forceVector, ForceMode forceMode)
        {
            Context = context;
            GameObject newProjectile = GameObject.Instantiate(ProjectilePrefab, position, Quaternion.identity);
            newProjectile.transform.LookAt(newProjectile.transform.position + forceVector);
            newProjectile.GetComponentInChildren<SphereCollider>().radius = ProjectileColliderSize;
            newProjectile.GetComponentInChildren<Rigidbody>().AddForce(forceVector, forceMode);
            newProjectile.GetComponentInChildren<AudioSource>().Play(FlyingSound);
        }

        public abstract bool FilterCollision(Collision touchedCollider);

        public abstract void HitProjectile(IProjectile projectileInterface, Collision touchedCollider);

        public virtual void DestroyProjectile(IProjectile projectileInterface)
        {
            Destroy(projectileInterface.GameObject);
        }

        public virtual void DestroyProjectileAfterInstantiation(IProjectile projectileInterface)
        {
            if (_isDestroyedAfterInstantiation)
            {
                Destroy(projectileInterface.GameObject, TimeToDestroyAfterInstantiation);
            }
        }

        #endregion
    }
}
