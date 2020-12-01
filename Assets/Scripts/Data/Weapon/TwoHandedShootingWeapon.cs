using UnityEngine;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateTwoHandedShooting", order = 0)]
    public sealed class TwoHandedShootingWeapon : TwoHandedWeaponData, IShoot
    {
        #region Fields

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private int _magazineSize;

        [SerializeField] private float _hitDistance;
        [SerializeField] private float _timeBetweenShots;
        [SerializeField] private float _reloadTime;

        [SerializeField] private string _aimingAnimationPostfix;
        [SerializeField] private string _reloadAnimationPostfix;

        private WeaponHitBoxBehavior LeftWeaponHitBoxBehavior;
        private WeaponHitBoxBehavior RightWeaponHitBoxBehavior;
        private Collider LeftWeaponCollider;
        private Collider RightWeaponCollider;
        private ParticleSystem LeftParticleSystem;
        private ParticleSystem RightParticleSystem;

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
                LeftWeaponHitBoxBehavior = objectOnSceneLeft.GetComponentsInChildren<WeaponHitBoxBehavior>()[0];
                LeftWeaponCollider = objectOnSceneLeft.GetComponentsInChildren<Collider>()[0];
                LeftParticleSystem = objectOnSceneLeft.GetComponentInChildren<ParticleSystem>();
            }

            if (objectOnSceneRight != null && 
                objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>() != null)
            {
                RightWeaponHitBoxBehavior = objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>()[0];
                RightWeaponCollider = objectOnSceneRight.GetComponentsInChildren<Collider>()[0];
                RightParticleSystem = objectOnSceneRight.GetComponentInChildren<ParticleSystem>();
            }
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
                    gunsPosition = LeftParticleSystem.transform.position;
                    forwardVector = LeftParticleSystem.transform.forward;
                    break;
                case HandsEnum.Right:
                    gunsPosition = RightParticleSystem.transform.position;
                    forwardVector = RightParticleSystem.transform.forward;
                    break;
                case HandsEnum.Both:
                    gunsPosition = (LeftParticleSystem.transform.position +
                    RightParticleSystem.transform.position) / 2;
                    forwardVector = (LeftParticleSystem.transform.forward +
                    RightParticleSystem.transform.forward) / 2;
                    break;
                default:
                    break;
            }

            Shoot(gunsPosition, forwardVector * HitDistance, CurrentAttack.AttackType);
        }

        public void Shoot(Vector3 gunPosition, Vector3 forwardDirection, HandsEnum inWhichHand)
        {
            RaycastHit rayHit;

            bool raycastCheck = Services.SharedInstance.PhysicsService.MakeRaycast(gunPosition,
                forwardDirection, out rayHit, HitDistance);               

            switch (inWhichHand)
            {
                case HandsEnum.Left:
                    LeftParticleSystem.Play();
                    break;
                case HandsEnum.Right:
                    RightParticleSystem.Play();
                    break;
                case HandsEnum.Both:
                    LeftParticleSystem.Play();
                    RightParticleSystem.Play();
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
                            if (LeftWeaponHitBoxBehavior != null) LeftWeaponHitBoxBehavior.IsInteractable = true;
                            enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                LeftWeaponCollider);
                            OnHit?.Invoke(LeftWeaponHitBoxBehavior as ITrigger, rayHit.collider);                          
                            break;
                        case HandsEnum.Right:
                            if (RightWeaponHitBoxBehavior != null) RightWeaponHitBoxBehavior.IsInteractable = true;
                            enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                RightWeaponCollider);
                            OnHit?.Invoke(RightWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            break;
                        case HandsEnum.Both:
                            if (LeftWeaponHitBoxBehavior != null) LeftWeaponHitBoxBehavior.IsInteractable = true;
                            if (RightWeaponHitBoxBehavior != null) RightWeaponHitBoxBehavior.IsInteractable = true;
                            if (LeftWeaponCollider != null)
                            {
                                enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                    LeftWeaponCollider);
                                OnHit?.Invoke(LeftWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            }
                            else
                            {
                                enemyBehavior.OnTriggerEnterHandler?.Invoke(enemyBehavior as ITrigger, 
                                    RightWeaponCollider);
                                OnHit?.Invoke(RightWeaponHitBoxBehavior as ITrigger, rayHit.collider);
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (LeftWeaponHitBoxBehavior != null) LeftWeaponHitBoxBehavior.IsInteractable = false;
                if (RightWeaponHitBoxBehavior != null) RightWeaponHitBoxBehavior.IsInteractable = false;
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

