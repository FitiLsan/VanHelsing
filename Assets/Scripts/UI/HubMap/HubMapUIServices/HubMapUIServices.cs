namespace BeastHunter
{
    public class HubMapUIServices
    {
        public static readonly HubMapUIServices SharedInstance = new HubMapUIServices();


        public HubMapUIShopService ShopService { get; private set; }
        public HubMapUITravelTimeService TravelTimeService { get; private set; }
        public HubMapUIItemInitializeService ItemInitializeService { get; private set; }


        public void InitializeServices()
        {
            ShopService = new HubMapUIShopService();
            TravelTimeService = new HubMapUITravelTimeService();
            ItemInitializeService = new HubMapUIItemInitializeService();
        }
    }
}
