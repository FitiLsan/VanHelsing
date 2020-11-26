using UnityEngine;


namespace BeastHunter
{
    public interface IShoot
    {
        #region Properties

        BulletType BulletType { get; }
        int MagazineSize { get; }

        float HitDistance { get; }
        float ReloadTime { get; }

        string AimingAnimationPostfix { get; }
        string ReloadAnimationPostfix { get; }

        #endregion


        #region Methods

        void Shoot(Vector3 gunPosition, Vector3 forwardDirection);

        void Reload();

        #endregion
    }
}

