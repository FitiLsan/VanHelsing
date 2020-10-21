using UnityEngine;


namespace BeastHunter
{
    public abstract class TwoHandedWeaponData : WeaponData
    {
        #region Fields

        public WeaponItem LeftActualWeapon;
        public WeaponItem RightActualWeapon;

        #endregion


        #region Methods

        public virtual void Init(GameObject objectOnSceneLeft, GameObject objectOnSceneRight)
        {
            LeftActualWeapon.WeaponObjectOnScene = objectOnSceneLeft;
            RightActualWeapon.WeaponObjectOnScene = objectOnSceneRight;
        }

        #endregion
    }
}

