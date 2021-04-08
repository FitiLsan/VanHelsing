using UnityEngine;
using DG.Tweening;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Character/CreateWeapon/CreateOneHandedThrowable", order = 0)]
    public sealed class OneHandedThrowableWeapon : OneHandedWeaponData
    {
        #region Fields

        [SerializeField] private ProjectileData _projectileData;
        [SerializeField] private float _delayBeforeThrow;
        [SerializeField] private float _hitDistance;
        [SerializeField] private string _aimingAnimationPostfix;

        #endregion


        #region Properties

        public ProjectileData ProjectileData => _projectileData;
        public float DelayBeforeThrow => _delayBeforeThrow;
        public float HitDistance => _hitDistance;
        public string AimingAnimationPostfix => _aimingAnimationPostfix;

        #endregion


        #region ClassLifeCycle

        public OneHandedThrowableWeapon()
        {
            _handType = WeaponHandType.OneHanded;
            _type = WeaponType.Throwing;
        }

        #endregion


        #region Methods

        public override void Init(GameObject objectOnScene)
        {
            base.Init(objectOnScene);
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

            DOVirtual.DelayedCall(_delayBeforeThrow, () => Throw(_context.CharacterModel.CurrentWeaponRight.
                transform.position, (bodyTransform.up.normalized + bodyTransform.forward) * HitDistance, 
                    CurrentAttack.AttackType));
        }

        public void Throw(Vector3 projectilePosition, Vector3 forwardDirection, HandsEnum inWhichHand)
        {
            new ProjectileInitializeController(_context, _projectileData, projectilePosition, 
                forwardDirection, ForceMode.Impulse);
            _context.InputModel.OnRemoveWeapon?.Invoke();
        }

        #endregion
    }
}

