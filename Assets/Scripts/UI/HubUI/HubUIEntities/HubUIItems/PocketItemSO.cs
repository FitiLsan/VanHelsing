using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "PocketItem", menuName = "CreateData/HubUIData/Items/PocketItem", order = 0)]
    public class PocketItemSO : BaseItemSO
    {
        #region Properties

        public override ItemType ItemType => ItemType.PocketItem;

        #endregion
    }
}
