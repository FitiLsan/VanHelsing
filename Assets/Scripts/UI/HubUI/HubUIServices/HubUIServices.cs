namespace BeastHunterHubUI
{
    public class HubUIServices
    {
        public static readonly HubUIServices SharedInstance = new HubUIServices();


        public ShopService ShopService { get; private set; }
        public ItemInitializeService ItemInitializeService { get; private set; }
        public MainInput MainInput { get; private set; }
        public GameMessages GameMessages { get;private set;}
        public TimeService TimeService { get; private set; }
        public HubUIEventsService EventsService { get; private set; }
        public CharacterCheckNameService CharacterCheckNameService { get; private set; }
        public HuntingQuestService HuntingQuestService { get; private set; }
        public RandomCharacterService RandomCharacterService { get; private set; }


        public void InitializeServices(HubUIContext context)
        {
            ShopService = new ShopService(context);
            ItemInitializeService = new ItemInitializeService();
            GameMessages = new GameMessages();
            TimeService = new TimeService(context);
            EventsService = new HubUIEventsService(context);
            CharacterCheckNameService = new CharacterCheckNameService(context);
            HuntingQuestService = new HuntingQuestService();
            RandomCharacterService = new RandomCharacterService();

            MainInput = new MainInput();
            MainInput.Enable();
        }

        public void DisposeGameServices()
        {
            MainInput.Disable();
        }
    }
}
