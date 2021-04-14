namespace BeastHunter
{
    public class HubMapUIPlayerModel
    {
        #region Properties

        public int GoldAmount { get; set; }
        public HubMapUIItemStorage Inventory { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HubMapUIPlayerModel(HubMapUIPlayerData data)
        {
            GoldAmount = data.GoldAmount;

            Inventory = new HubMapUIItemStorage(data.InventorySlotsAmount, HubMapUIItemStorageType.GeneralInventory);
            for (int i = 0; i < data.InventoryItems.Length; i++)
            {
                HubMapUIBaseItemModel itemModel = HubMapUIServices.SharedInstance.
                    ItemInitializeService.InitializeItemModel(data.InventoryItems[i]);
                Inventory.PutItem(i, itemModel);
            }
        }

        #endregion
    }
}
