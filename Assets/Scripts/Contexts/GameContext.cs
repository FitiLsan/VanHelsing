using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeastHunter
{
    public sealed class GameContext : Contexts
    {
        #region Fields

        public StartDialogueModel StartDialogueModel;
        public DialogueSystemModel DialogueSystemModel;
        public QuestModel QuestModel;
        public SphereModel SphereModel;
        public BossModel BossModel;
        public CharacterModel CharacterModel;
        public InputModel InputModel;
        public GiantMudCrabModel GiantMudCrabModel;
        public RabbitModel RabbitModel;
		public List<QuestIndicatorModel> QuestIndicatorModelList = new List<QuestIndicatorModel>();
        public QuestJournalModel QuestJournalModel;
        public Dictionary<int, EnemyModel> NpcModels;
        public Dictionary<int, TrapModel> TrapModels;

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
        }

        #endregion


        #region Methods

        public void AddTriggers(InteractableObjectType InteractionType, ITrigger TriggerInterface)
        {
            if (!_interactables.Contains(TriggerInterface))
            {
                _interactables.Add(TriggerInterface);
            }

            if (_onTriggers.ContainsKey(InteractionType))
            {
                _onTriggers[InteractionType].Add(TriggerInterface);
            }
            else
            {
                _onTriggers.Add(InteractionType, new List<IInteractable>
                {
                    TriggerInterface
                });
            }

            TriggerInterface.DestroyHandler = DestroyHandler;
            AddObjectHandler.Invoke(TriggerInterface);
        }

        private void DestroyHandler(ITrigger TriggerInterface, InteractableObjectType InteractionType)
        {
            _onTriggers[InteractionType].Remove(TriggerInterface);
            _interactables.Remove(TriggerInterface);
        }

        public List<T> GetTriggers<T>(InteractableObjectType InteractionType) where T : class, IInteractable
        {
            return _onTriggers.ContainsKey(InteractionType) ? _onTriggers[InteractionType].Select(trigger => trigger as T).ToList() : null;
        }

        public List<IInteractable> GetTriggers(InteractableObjectType InteractionType)
        {
            return _onTriggers.ContainsKey(InteractionType) ? _onTriggers[InteractionType] : _onTriggers[InteractionType] = new List<IInteractable>();
        }

        public List<IInteractable> GetListInteractable()
        {
            return _interactables;
        }

        #endregion
    }
}
