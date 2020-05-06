using UnityEngine;


namespace BeastHunter
{
    public sealed class DialogueTriggerController : IAwake, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private Collider _target;

        #endregion


        #region ClassLifeCycles

        public DialogueTriggerController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var npcs = _context.GetTriggers(InteractableObjectType.Npc);
            foreach (var trigger in npcs)
            {
                var dialogueBehavior = trigger as DialogueBehavior;
                dialogueBehavior.OnFilterHandler += OnFilterHandler;
                dialogueBehavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
                dialogueBehavior.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var npcs = _context.GetTriggers(InteractableObjectType.Npc);
            foreach (var trigger in npcs)
            {
                var dialogueBehavior = trigger as DialogueBehavior;
                dialogueBehavior.OnFilterHandler -= OnFilterHandler;
                dialogueBehavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                dialogueBehavior.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.NPC);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject, Collider other)
        {
            enteredObject.IsInteractable = true;
            _context.StartDialogueModel.StartDialogueData.OnTriggerEnter(other);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject, Collider other)
        {
            enteredObject.IsInteractable = false;
            _context.StartDialogueModel.StartDialogueData.OnTriggerExit(other);
        }

        #endregion
    }

}
