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
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
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
