using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "CreateWeapon/CreateTwoHandedMelee", order = 0)]
    public class TwoHandedMeleeWeapon : TwoHandedWeaponData
    {
        #region ClassLifeCycle

        public TwoHandedMeleeWeapon()
        {
            HandType = WeaponHandType.TwoHanded;
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

