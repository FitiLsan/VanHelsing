namespace BeastHunter
{
    public class HubMapUIItemInitializeService
    {
        public HubMapUIBaseItemModel InitializeItemModel(HubMapUIBaseItemData data)
        {
            switch (data.ItemType)
            {
                case HubMapUIItemType.None:
                    return new HubMapUIBaseItemModel(data);

                case HubMapUIItemType.Cloth:
                    return new HubMapUIClothesItemModel(data);

                //todo other cases

                default:
                    CustomDebug.LogError(this + ": incorrect HubMapUIItemType");
                    return null;
            }
        }

    }
}
