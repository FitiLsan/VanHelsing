using UnityEngine;


namespace BeastHunter
{
    public interface IShoot
    {
        #region Properties

        ProjectileData ProjectileData { get; }
        int MagazineSize { get; }

        float HitDistance { get; }
        float TimeBetweenShots { get; }
        float ReloadTime { get; }

        string AimingAnimationPostfix { get; }
        string ReloadAnimationPostfix { get; }

        #endregion


        #region Methods

        void Shoot(Vector3 gunPosition, Vector3 forwardDirection, HandsEnum inWhichHand);

        void Reload();

        #endregion
    }
}

