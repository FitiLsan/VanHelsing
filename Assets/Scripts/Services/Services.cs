namespace BeastHunter
{
    public sealed class Services
    {
        public static readonly Services SharedInstance = new Services();

        #region Properties

        public PhysicsService PhysicsService { get; private set; }
        public InventoryService InventoryService { get; private set; }
        public AttackService AttackService { get; private set; }
        public CameraService CameraService { get; private set; }
        public UnityTimeService UnityTimeService { get; private set; }
        public EventManager EventManager { get; private set; }
        public BuffService BuffService { get; private set; }
        public TimeSkipService TimeSkipService { get; private set; }
        public TrapService TrapService { get; private set; }
        public NoiseService NoiseService { get; private set; }
        public AudioService AudioService { get; private set; }
        public InitializationService InitializationService { get; private set; }

        #endregion


        #region Methods

        public void InitializeGameServices(GameContext context)
        {
            PhysicsService = new PhysicsService();
          //  InventoryService = new InventoryService();
            AttackService = new AttackService(context);
            CameraService = new CameraService(context);
            EventManager = new EventManager();
            BuffService = new BuffService();
            UnityTimeService = new UnityTimeService();
            TimeSkipService = new TimeSkipService();
            TrapService = new TrapService(context);
            NoiseService = new NoiseService();
            AudioService = new AudioService(context);
            InitializationService = new InitializationService(context); //dispose?
        }

        public void DisposeGameServices()
        {
            CameraService.Dispose();
            AudioService.Dispose();
        }

        #endregion
    }
}
