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
            var DialogueSystemData = Data.DialogueSystemData;
            GameObject instance = GameObject.Instantiate(DialogueSystemData.DialogueSystemStruct.Prefab);
            DialogueSystemModel DialogueSystem = new DialogueSystemModel(instance, DialogueSystemData);
            _context._dialogueSystemModel = DialogueSystem;
        }

        #endregion
    }
}