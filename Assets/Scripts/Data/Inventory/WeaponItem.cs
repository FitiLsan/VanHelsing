using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewWeaponItem", menuName = "CreateItem/CreateWeaponItem", order = 0)]
    public sealed class WeaponItem : BaseItem
    {
        #region Fields
         
        public GameObject WeaponPrefab;
        public GameObject WeaponObjectOnScene;

        [Tooltip("Weapon weight.")]
        public float Weight;

        #endregion
    }
}


