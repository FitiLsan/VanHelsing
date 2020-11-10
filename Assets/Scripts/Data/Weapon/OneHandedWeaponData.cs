using UnityEngine;


namespace BeastHunter
{
    public abstract class OneHandedWeaponData : WeaponData
    {
        #region Fields

        public WeaponItem ActualWeapon;

        #endregion


        #region Methods

        public virtual void Init(GameObject objectOnScene)
        {
            ActualWeapon.WeaponObjectOnScene = objectOnScene;
        }

        #endregion
    }
}

