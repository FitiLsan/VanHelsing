using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "PocketItem", menuName = "CreateData/HubUIData/Items/PocketItem", order = 0)]
    class PocketItemData : BaseItemData
    {
        #region Properties

        public override ItemType ItemType { get; protected set; }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            ItemType = ItemType.PocketItem;
        }

        #endregion
    }
}
