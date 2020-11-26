using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateOneHandedShooting", order = 0)]
    public sealed class OneHandedShootingWeapon : OneHandedWeaponData, IShoot
    {
        #region Fields

        [SerializeField] private BulletType _bulletType;
        [SerializeField] private int _magazineSize;

        [SerializeField] private float _hitDistance;
        [SerializeField] private float _reloadTime;

        [SerializeField] private string _aimingAnimationPostfix;
        [SerializeField] private string _reloadAnimationPostfix;

        #endregion


        #region Properties

        public BulletType BulletType => _bulletType;
        public int MagazineSize => _magazineSize;
        public float HitDistance => _hitDistance;
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

        public void Shoot(Vector3 gunPosition, Vector3 forwardDirection)
        {
            // TODO
        }

        #endregion
    }
}

