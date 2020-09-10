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

        #endregion


        #region ClassLifeCycle

        public TwoHandedShootingWeapon()
        {
            HandType = WeaponHandType.TwoHanded;
            Type = WeaponType.Shooting;
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

        #endregion
    }
}

