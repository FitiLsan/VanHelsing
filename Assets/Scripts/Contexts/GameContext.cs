using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class GameContext : Contexts
    {
        #region Fields
        
        public UIBestiaryModel UIBestiaryModel;
        public StartDialogueModel StartDialogueModel;
        public DialogueSystemModel DialogueSystemModel;
        public QuestModel QuestModel;
        public BossModel BossModel;
        public CharacterModel CharacterModel;
        public InputModel InputModel;
		public List<QuestIndicatorModel> QuestIndicatorModelList = new List<QuestIndicatorModel>();
        public QuestJournalModel QuestJournalModel;

        public Dictionary<int, EnemyModel> NpcModels;
        public Dictionary<int, TrapModel> TrapModels;
        public Dictionary<int, BaseInteractiveObjectModel> InteractableObjectModels;

        public event Action<IInteractable> AddObjectHandler = delegate (IInteractable interactable) { };
        private readonly SortedList<InteractableObjectType, List<IInteractable>> _onTriggers;
        private readonly List<IInteractable> _interactables;

        #endregion


        #region ClassLifeCycles

        public GameContext()
        {
            _onTriggers = new SortedList<InteractableObjectType, List<IInteractable>>();
            _interactables = new List<IInteractable>();

            NpcModels = new Dictionary<int, EnemyModel>();
            InteractableObjectModels = new Dictionary<int, BaseInteractiveObjectModel>();
        }

        #endregion
    }
}
