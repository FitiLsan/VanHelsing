using UnityEngine;


namespace BeastHunter
{
    public sealed class DialogueSystemInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public DialogueSystemInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var dialogueSystemData = Data.DialogueSystemData;
            GameObject instance = GameObject.Instantiate(dialogueSystemData.DialogueSystemStruct.Prefab);
            DialogueSystemModel dialogueSystem = new DialogueSystemModel(instance, dialogueSystemData);
            _context._dialogueSystemModel = dialogueSystem;
        }

        #endregion
    }
}