namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context, GameControllerParametersData controllerData)
        {
            AddInitializeControllers(context, controllerData);
            AddControllers(context, controllerData);
        }

        #endregion


        #region Methods

        private void AddInitializeControllers(GameContext context, GameControllerParametersData controllerData) 
        {      
            Add(new LocationInitializeController(context, controllerData.DoImplementPlayer, controllerData.DoImplementBoss,
                controllerData.DoImplementMobs, controllerData.DoImplementInteractiveObjects));
            if (controllerData.DoImplementDialogSystem)
            {
                Add(new DialogueSystemInitializeController(context));
                Add(new StartDialogueInitializeController(context));
            }
            if (controllerData.DoImplementQuestSystem)
            {
                Add(new QuestInitializeController(context));
                Add(new QuestIndicatorInitializeController(context));
                Add(new QuestJournalInitializeController(context));
            }
            if (controllerData.DoImplementPlayer) Add(new UIBestiarylInitializeController(context));
        }

        private void AddControllers(GameContext context, GameControllerParametersData controllerData)
        {
            Add(new TimeRemainingController(context));
            if (controllerData.DoImplementBoss || controllerData.DoImplementMobs) Add(new EnemyController(context));
            if (controllerData.DoImplementInput) Add(new InputController(context));
            if (controllerData.DoImplementPlayer)
            {
                Add(new CharacterController(context));
                Add(new CharacterAnimationController(context));
                Add(new TrapController(context));
            }
            if (controllerData.DoImplementInteractiveObjects) Add(new InteractiveObjectController(context));
            if (controllerData.DoImplementDialogSystem)
            {
                Add(new DialogueSystemController(context));
                Add(new StartDialogueController(context));
                Add(new DialogueTriggerController(context));
            }
            if (controllerData.DoImplementQuestSystem)
            {
                Add(new QuestController(context));
                Add(new QuestIndicatorController(context));
            }
            if (controllerData.DoImplementPlayer) Add(new UIBestiaryController(context));
        }

        #endregion
    }
}
