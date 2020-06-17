using UnityEngine;


namespace BeastHunter
{
    public sealed class DialogueSystemInitializeController : IAwake
    {
        #region Field

        GameContext _context;
        GameObject instance;
        #endregion


        #region ClassLifeCycle

        public DialogueSystemInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var dialogueSystemData = Data.DialogueSystemData;
            var inst = GameObject.Find("MapDialogueSystem");
            if (inst == null)
                
            {
                instance = GameObject.Instantiate(dialogueSystemData.DialogueSystemStruct.Prefab);
            }
            instance = inst;
                DialogueSystemModel dialogueSystem = new DialogueSystemModel(instance, dialogueSystemData);
                _context.DialogueSystemModel = dialogueSystem;
            
        }

        #endregion
    }
}