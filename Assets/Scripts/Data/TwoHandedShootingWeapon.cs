using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateTwoHandedShooting", order = 0)]
    public class TwoHandedShootingWeapon : TwoHandedWeaponData
    {
        #region Fields

        public BulletType BulletType;
        public int MagazineSize;

        public float HitDistance;
        public float ReloadTime;

        public string ReloadAnimationPrefix;

        private WeaponHitBoxBehavior LeftWeaponHitBoxBehavior;
        private WeaponHitBoxBehavior RightWeaponHitBoxBehavior;
        private Collider LeftWeaponCollider;
        private Collider RightWeaponCollider;

        #endregion


        #region ClassLifeCycle

        public TwoHandedShootingWeapon()
        {
            HandType = WeaponHandType.TwoHanded;
            Type = WeaponType.Shooting;
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
            }

            if (objectOnSceneRight != null && 
                objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>() != null)
            {
                RightWeaponHitBoxBehavior = objectOnSceneRight.GetComponentsInChildren<WeaponHitBoxBehavior>()[0];
                RightWeaponCollider = objectOnSceneRight.GetComponentsInChildren<Collider>()[0];
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

            switch (InWhichHand)
            {
                case HandsEnum.Left:
                    gunsPosition = LeftActualWeapon.WeaponObjectOnScene.transform.position;
                    break;
                case HandsEnum.Right:
                    gunsPosition = RightActualWeapon.WeaponObjectOnScene.transform.position;
                    break;
                case HandsEnum.Both:
                    gunsPosition = (LeftActualWeapon.WeaponObjectOnScene.transform.position +
                    RightActualWeapon.WeaponObjectOnScene.transform.position) / 2;
                    break;
                default:
                    break;
            }

            TimeRemaining shootTime = new TimeRemaining(() => Shoot(gunsPosition, BodyTransform.forward), 
                CurrentAttack.AttackTime / 2);
            shootTime.AddTimeRemaining(CurrentAttack.AttackTime / 2);
        }

        private void Shoot(Vector3 gunPosition, Vector3 forwardDirection)
        {
            RaycastHit rayHit;

            bool raycastCheck = Services.SharedInstance.PhysicsService.MakeRaycast(gunPosition,
                forwardDirection, out rayHit, HitDistance);

            if (raycastCheck)
            {
                InteractableObjectBehavior enemyBehavior = rayHit.collider.GetComponent<InteractableObjectBehavior>();

                if(enemyBehavior != null)
                {
                    switch (InWhichHand)
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

