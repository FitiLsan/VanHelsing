using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateOneHandedShooting", order = 0)]
    public sealed class OneHandedShootingWeapon : OneHandedWeaponData, IShoot
    {
        #region Fields

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private int _magazineSize;

        [SerializeField] private float _hitDistance;
        [SerializeField] private float _timeBetweenShots;
        [SerializeField] private float _reloadTime;

        [SerializeField] private string _aimingAnimationPostfix;
        [SerializeField] private string _reloadAnimationPostfix;

        private ParticleSystem _particleSystem;

        #endregion


        #region Properties

        public ProjectileData ProjectileData => _projectileData;
        public int MagazineSize => _magazineSize;
        public float HitDistance => _hitDistance;
        public float TimeBetweenShots => _timeBetweenShots;
        public float ReloadTime => _reloadTime;
        public string AimingAnimationPostfix => _aimingAnimationPostfix;
        public string ReloadAnimationPostfix => _reloadAnimationPostfix;

        #endregion


        #region ClassLifeCycle

        public OneHandedShootingWeapon()
        {
            _handType = WeaponHandType.OneHanded;
            _type = WeaponType.Shooting;
        }

        #endregion


        #region Methods

        public override void Init(GameObject objectOnScene)
        {
            base.Init(objectOnScene);

            _particleSystem = objectOnScene.GetComponentInChildren<ParticleSystem>();
        }

        public override void TakeWeapon()
        {
            base.TakeWeapon();
        }

        public override void LetGoWeapon()
        {
            base.LetGoWeapon();
        }

        public void Reload()
        {
            if (IsInHands)
            {
                // TODO
            }
        }

        public override void MakeSimpleAttack(out int currentAttackIntex, Transform bodyTransform)
        {
            base.MakeSimpleAttack(out currentAttackIntex, bodyTransform);

            Shoot(_particleSystem.transform.position, _particleSystem.transform.forward * HitDistance, 
                CurrentAttack.AttackType);
        }

        public void Shoot(Vector3 gunPosition, Vector3 forwardDirection, HandsEnum inWhichHand)
        {
            new ProjectileInitializeController(_context, _projectileData, gunPosition, forwardDirection, ForceMode.Impulse);
            _particleSystem.Play();
        }

        #endregion
    }
}

