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

        #endregion


        #region Methods

        public void Initialize(Contexts contexts)
        {
            PhysicsService = new PhysicsService(contexts);
            InventoryService = new InventoryService(contexts);
            AttackService = new AttackService(contexts);
            CameraService = new CameraService(contexts);
            EventManager = new EventManager(contexts);
            BuffService = new BuffService(contexts);
            UnityTimeService = new UnityTimeService(contexts);
            TimeSkipService = new TimeSkipService(contexts);
            TrapService = new TrapService(contexts);
        }

        #endregion
    }
}
