namespace BeastHunter
{
    public class HubMapUIServices
    {
        public static readonly HubMapUIServices SharedInstance = new HubMapUIServices();


        public HubMapUIShopService ShopService { get; private set; }
        public HubMapUITravelTimeService TravelTimeService { get; private set; }
        public HubMapUIItemInitializeService ItemInitializeService { get; private set; }
        public MainInput MainInput { get; private set; }
        public HubMapUIGameMessages GameMessages { get;private set;}


        public void InitializeServices()
        {
            ShopService = new HubMapUIShopService();
            TravelTimeService = new HubMapUITravelTimeService();
            ItemInitializeService = new HubMapUIItemInitializeService();
            MainInput = new MainInput();
            MainInput.Enable(); //todo: inputController instead mainInput
            GameMessages = new HubMapUIGameMessages();
        }
    }
}
