namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            AddInitializeControllers(context);
            //Add(new InitializeInteractableObjectController(context));
            AddControllers(context);

            //Add(new QuestIndicatorInitializeController(context));
            //Add(new QuestJournalInitializeController(context));
            //Add(new QuestIndicatorController(context));
            //Add(new QuestIndicatorController(context));
        }

        #endregion


        #region Methods

        private void AddInitializeControllers(GameContext context) 
        {
            //Add(new IOInitializeController(context));
            //Add<T>(context) where T : NpcInitializeController

            Add(new CharacterInitializeController(context));
            Add(new BossInitializeController(context));
            //Add(new GiantMudCrabInitilizeController(context));
            //Add(new RabbitInitializeController(context));
            //Add(new DialogueSystemInitializeController(context));
            //Add(new StartDialogueInitializeController(context));
            //Add(new QuestInitializeController(context));
        }

        private void AddControllers(GameContext context)
        {
            Add(new EnemyController(context));
            Add(new TrapController(context));
            //Add(new GiantMudCrabController(context));
            Add(new TargetController(context));
            Add(new InputController(context));
            Add(new TimeRemainingController(context));
            Add(new CharacterController(context));
            //Add(new DialogueSystemController(context));
            //Add(new StartDialogueController(context));
            //Add(new DialogueTriggerController(context));
            //Add(new QuestController(context));
        }

        #endregion
    }
}
