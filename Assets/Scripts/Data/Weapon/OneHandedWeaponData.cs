using UnityEngine;


namespace BeastHunter
{
    public abstract class OneHandedWeaponData : WeaponData
    {
        #region Fields

        [SerializeField] protected WeaponItem _actualWeapon;

        #endregion


        #region Properties

        public WeaponItem ActualWeapon => _actualWeapon;

        #endregion


        #region Methods

        public virtual void Init(GameObject objectOnScene)
        {
            ActualWeapon.WeaponObjectOnScene = objectOnScene;
        }

        #endregion
    }
}

