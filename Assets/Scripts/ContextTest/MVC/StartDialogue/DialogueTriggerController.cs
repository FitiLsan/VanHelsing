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

        public DialogueTriggerController(GameContext context, Services services)
        {
            this._context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var sphers = _context.GetTriggers(InteractableObjectType.Npc);
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
            var npcs = _context.GetTriggers(InteractableObjectType.Npc);
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
            _target = tagObject;
            return tagObject.CompareTag(TagManager.NPC);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
            _context._startDialogueModel.StartDialogueData.OnTriggerEnter(_target);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            _context._startDialogueModel.StartDialogueData.OnTriggerExit(_target);
        }

        #endregion
    }

}
