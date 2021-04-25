using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "HubUIWeaponItem", menuName = "CreateData/HubUIData/Items/Weapon", order = 0)]
    public class WeaponItemData : BaseItemData
    {
        #region Fields

        [SerializeField] private bool _isTwoHanded;

        #endregion


        #region Properties

        public bool IsTwoHanded => _isTwoHanded;
        public override ItemType ItemType { get; protected set; }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            ItemType = ItemType.Weapon;
        }

        #endregion
    }
}
