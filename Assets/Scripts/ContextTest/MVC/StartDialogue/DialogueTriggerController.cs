using UnityEngine;


namespace BeastHunter
{
    public sealed class DialogueTriggerController : IAwake, ITearDown
    {
        #region Fields

        private readonly GameContext context;
        private Collider target;

        #endregion


        #region ClassLifeCycles

        public DialogueTriggerController(GameContext context, Services services)
        {
            this.context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var sphers = context.GetTriggers(InteractableObjectType.Npc);
            foreach (var trigger in sphers)
            {
                var targetBehaviour = trigger as TargetBehaviour;
                targetBehaviour.OnFilterHandler += OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var npcs = context.GetTriggers(InteractableObjectType.Npc);
            foreach (var trigger in npcs)
            {
                var targetBehaviour = trigger as TargetBehaviour;
                targetBehaviour.OnFilterHandler -= OnFilterHandler;
                targetBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                targetBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            target = tagObject;
            return tagObject.CompareTag(TagManager.NPC);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
            Debug.Log($"Enter NPC Dialogue {target.name}");
            context._startDialogueModel.StartDialogueData.OnTriggerEnter(target);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            Debug.Log($"Exit NPC Dialogue {target.name}");
            context._startDialogueModel.StartDialogueData.OnTriggerExit(target);
        }

        #endregion
    }

}
