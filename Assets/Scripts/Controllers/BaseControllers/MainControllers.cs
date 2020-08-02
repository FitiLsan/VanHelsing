﻿namespace BeastHunter
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context)
        {
            //   Add(new CharacterInitilizeController(context));
            //   Add(new GiantMudCrabInitilizeController(context));
            Add(new DialogueSystemInitializeController(context));
            Add(new StartDialogueInitializeController(context));
            //    Add(new InitializeInteractableObjectController(context));
                   Add(new QuestInitializeController(context));
                 Add(new QuestIndicatorInitializeController(context));
                 Add(new QuestJournalInitializeController(context));
            Add(new UIIndicationInitializeController(context));
            Add(new PlaceSearcherController(context));
            //     Add(new GiantMudCrabController(context));
            //     Add(new TargetController(context));
            //      Add(new InputController(context));
            //     Add(new CharacterController(context));
            Add(new DialogueSystemController(context));
            Add(new StartDialogueController(context));
            //    Add(new DialogueTriggerController(context));
              Add(new QuestController(context));
            // Add(new RabbitInitializeController(context, Services.SharedInstance));
            //Add(new RabbitController(context, Services.SharedInstance));
              Add(new QuestIndicatorController(context));
            Add(new QuestJournalController(context));
            Add(new UIIndicationController(context));
        }

        #endregion
    }
}
