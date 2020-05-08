namespace BeastHunter
{
    public sealed class Services
    {
        public static readonly Services SharedInstance = new Services();

        #region Properties

        public PhysicsService PhysicsService { get; private set; }
        public InventoryService InventoryService { get; private set; }
        public AttackService AttackService { get; private set; }

        #endregion


        #region Methods

        public void Initialize(Contexts contexts)
        {
            PhysicsService = new PhysicsService(contexts);
            InventoryService = new InventoryService(contexts);
            AttackService = new AttackService(contexts);
        }

        #endregion
    }
}
