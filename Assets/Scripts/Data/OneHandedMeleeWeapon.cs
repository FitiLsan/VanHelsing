using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateOneHandedMelee", order = 0)]
    public class OneHandedMeleeWeapon : OneHandedWeaponData
    {
        #region ClassLifeCycle

        public OneHandedMeleeWeapon()
        {
            HandType = WeaponHandType.OneHanded;
            Type = WeaponType.Melee;
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

        #endregion
    }
}

