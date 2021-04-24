namespace BeastHunterHubUI
{
    public class HubUIServices
    {
        public static readonly HubUIServices SharedInstance = new HubUIServices();


        public ShopService ShopService { get; private set; }
        public TravelTimeService TravelTimeService { get; private set; }
        public ItemInitializeService ItemInitializeService { get; private set; }
        public MainInput MainInput { get; private set; }
        public GameMessages GameMessages { get;private set;}
        public OrdersService OrdersService { get; private set; }
        public HubUIEventsService EventsService { get; private set; }


        public void InitializeServices(HubUIContext context)
        {
            ShopService = new ShopService(context);
            TravelTimeService = new TravelTimeService();
            ItemInitializeService = new ItemInitializeService();
            GameMessages = new GameMessages();
            OrdersService = new OrdersService(context);
            EventsService = new HubUIEventsService(context);

            MainInput = new MainInput();
            MainInput.Enable();
        }

        public void DisposeGameServices()
        {
            MainInput.Disable();
        }
    }
}
