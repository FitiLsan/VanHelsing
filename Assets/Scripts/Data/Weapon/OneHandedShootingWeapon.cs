using UnityEngine;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateOneHandedShooting", order = 0)]
    public sealed class OneHandedShootingWeapon : OneHandedWeaponData, IShoot
    {
        #region Fields

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private Sound _shootingSound;
        [SerializeField] private Sound _reloadingSound;

        [SerializeField] private int _magazineSize;

        [SerializeField] private float _hitDistance;
        [SerializeField] private float _timeBetweenShots;
        [SerializeField] private float _reloadTime;

        [SerializeField] private string _aimingAnimationPostfix;
        [SerializeField] private string _reloadAnimationPostfix;

        private AudioSource _weaponAudioSource;

        #endregion


        #region Properties

        public ParticleSystem ParticleSystem { get; private set; }
        public ProjectileData ProjectileData => _projectileData;
        public Sound ShootingSound => _shootingSound;
        public Sound ReloadingSound => _reloadingSound;
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

            ParticleSystem = objectOnScene.GetComponentInChildren<ParticleSystem>();
            _weaponAudioSource = objectOnScene.GetComponentInChildren<AudioSource>();
            _weaponAudioSource.PlayOneShot(GettingSound);
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

            Shoot(ParticleSystem.transform.position, (ParticleSystem.transform.forward) * HitDistance, CurrentAttack.AttackType);
        }

        public void Shoot(Vector3 gunPosition, Vector3 forwardDirection, HandsEnum inWhichHand)
        {
            new ProjectileInitializeController(_context, _projectileData, gunPosition, forwardDirection, ForceMode.Impulse);
            ParticleSystem.Play();
            _weaponAudioSource.PlayOneShot(ShootingSound);
        }

        #endregion
    }
}

