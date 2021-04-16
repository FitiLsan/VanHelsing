using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapUIPocketItemData", menuName = "CreateData/HubMapUIData/Items/PocketItem", order = 0)]
    class HubMapUIPocketItemData : HubMapUIBaseItemData
    {
        public override HubMapUIItemType ItemType { get; protected set; }

        private void OnEnable()
        {
            ItemType = HubMapUIItemType.PocketItem;
        }
    }
}
