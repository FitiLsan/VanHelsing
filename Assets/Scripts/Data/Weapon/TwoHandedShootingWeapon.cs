using UnityEngine;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateTwoHandedShooting", order = 0)]
    public sealed class TwoHandedShootingWeapon : TwoHandedWeaponData, IShoot
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

        private WeaponHitBoxBehavior _leftWeaponHitBoxBehavior;
        private WeaponHitBoxBehavior _rightWeaponHitBoxBehavior;
        private Collider _leftWeaponCollider;
        private Collider _rightWeaponCollider;
        private ParticleSystem _leftParticleSystem;
        private ParticleSystem _rightParticleSystem;
        private AudioSource _leftAudioSource;
        private AudioSource _rightAudioSource;

        #endregion


        #region Properties

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

        public TwoHandedShootingWeapon()
        {
            _handType = WeaponHandType.TwoHanded;
            _type = WeaponType.Shooting;
        }

        #endregion


        #region Methods

        public override void Init(GameObject objectOnSceneLeft, GameObject objectOnSceneRight)
        {
            base.Init(objectOnSceneLeft, objectOnSceneRight);

            if(objectOnSceneLeft != null && 
                objectOnSceneLeft.GetComponentsInChildren<WeaponHitBoxBehavior>() != null)
            {
                _leftWeaponHitBoxBehavior = objectOnSceneLeft.GetComponentsInChildren<WeaponHitBoxBehavior>()[0];
                _leftWeaponCollider = objectOnSceneLeft.GetComponentsInChildren<Collider>()[0];
                _leftParticleSystem = objectOnSceneLeft.GetComponentInChildren<ParticleSystem>();
                _leftAudioSource = objectOnSceneLeft.GetComponentInChildren<AudioSource>();
            }

            if (objectOnSceneRight != null && 
                objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>() != null)
            {
                _rightWeaponHitBoxBehavior = objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>()[0];
                _rightWeaponCollider = objectOnSceneRight.GetComponentsInChildren<Collider>()[0];
                _rightParticleSystem = objectOnSceneRight.GetComponentInChildren<ParticleSystem>();
                _rightAudioSource = objectOnSceneRight.GetComponentInChildren<AudioSource>();
            }

            _leftAudioSource.PlayOneShot(GettingSound);
            _rightAudioSource.PlayOneShot(GettingSound);
        }

        public override void TakeWeapon()
        {
            base.TakeWeapon();
        }

        public override void LetGoWeapon()
        {
            base.LetGoWeapon();
        }

        public override void MakeSimpleAttack(out int currentAttackIntex, Transform bodyTransform)
        {
            base.MakeSimpleAttack(out currentAttackIntex, bodyTransform);

            Vector3 gunsPosition = new Vector3();
            Vector3 forwardVector = new Vector3();

            switch (CurrentAttack.AttackType)
            {
                case HandsEnum.Left:
                    gunsPosition = _leftParticleSystem.transform.position;
                    forwardVector = _leftParticleSystem.transform.forward;
                    break;
                case HandsEnum.Right:
                    gunsPosition = _rightParticleSystem.transform.position;
                    forwardVector = _rightParticleSystem.transform.forward;
                    break;
                case HandsEnum.Both:
                    gunsPosition = (_leftParticleSystem.transform.position +
                    _rightParticleSystem.transform.position) / 2;
                    forwardVector = (_leftParticleSystem.transform.forward +
                    _rightParticleSystem.transform.forward) / 2;
                    break;
                default:
                    break;
            }

            Shoot(gunsPosition, Services.SharedInstance.CameraService.CharacterCamera.transform.forward * 
                HitDistance, CurrentAttack.AttackType);
        }

        public void Shoot(Vector3 gunPosition, Vector3 forwardDirection, HandsEnum inWhichHand)
        {
            RaycastHit rayHit;

            bool raycastCheck = Services.SharedInstance.PhysicsService.MakeRaycast(gunPosition,
                forwardDirection, out rayHit, HitDistance);               

            switch (inWhichHand)
            {
                case HandsEnum.Left:
                    _leftParticleSystem.Play();
                    _leftAudioSource.PlayOneShot(ShootingSound);
                    break;
                case HandsEnum.Right:
                    _rightParticleSystem.Play();
                    _rightAudioSource.PlayOneShot(ShootingSound);
                    break;
                case HandsEnum.Both:
                    _leftParticleSystem.Play();
                    _rightParticleSystem.Play();
                    _leftAudioSource.PlayOneShot(ShootingSound);
                    _rightAudioSource.PlayOneShot(ShootingSound);
                    break;
                default:
                    break;
            }

            if (raycastCheck)
            {
                InteractableObjectBehavior enemyBehavior = rayHit.transform.GetMainParent().GetComponent<InteractableObjectBehavior>();

                if(enemyBehavior != null)
                {
                    switch (inWhichHand)
                    {
                        case HandsEnum.Left:
                            if (_leftWeaponHitBoxBehavior != null) _leftWeaponHitBoxBehavior.IsInteractable = true;
                            enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                _leftWeaponCollider);
                            OnHit?.Invoke(_leftWeaponHitBoxBehavior as ITrigger, rayHit.collider);                          
                            break;
                        case HandsEnum.Right:
                            if (_rightWeaponHitBoxBehavior != null) _rightWeaponHitBoxBehavior.IsInteractable = true;
                            enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                _rightWeaponCollider);
                            OnHit?.Invoke(_rightWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            break;
                        case HandsEnum.Both:
                            if (_leftWeaponHitBoxBehavior != null) _leftWeaponHitBoxBehavior.IsInteractable = true;
                            if (_rightWeaponHitBoxBehavior != null) _rightWeaponHitBoxBehavior.IsInteractable = true;
                            if (_leftWeaponCollider != null)
                            {
                                enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                    _leftWeaponCollider);
                                OnHit?.Invoke(_leftWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            }
                            else
                            {
                                enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                    _rightWeaponCollider);
                                OnHit?.Invoke(_rightWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (_leftWeaponHitBoxBehavior != null) _leftWeaponHitBoxBehavior.IsInteractable = false;
                if (_rightWeaponHitBoxBehavior != null) _rightWeaponHitBoxBehavior.IsInteractable = false;
            }
        }

        public void Reload()
        {
            if (IsInHands)
            {
                // TODO
            }
        }

        #endregion
    }
}

