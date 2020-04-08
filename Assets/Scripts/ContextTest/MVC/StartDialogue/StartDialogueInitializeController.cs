using UnityEngine;


namespace BeastHunter
{
    public sealed class StartDialogueInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public StartDialogueInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var StartDialogueData = Data.StartDialogueData;
            GameObject instance = GameObject.Instantiate(StartDialogueData.StartDialogueStruct.Prefab,StartDialogueData.StartDialogueStruct.PlayerTransform);
            GameObject canvasNpc = GameObject.Instantiate(StartDialogueData.StartDialogueStruct.PrefabCanvasNpc);
            StartDialogueModel StartDialogue = new StartDialogueModel(instance, canvasNpc, StartDialogueData, _context);
            _context._startDialogueModel = StartDialogue;

            StartDialogueData.SetPerent(instance.transform, StartDialogueData.GetParent());
            StartDialogueData.GetDialogueSystemModel(_context._dialogueSystemModel);

        }

        #endregion
    }
}
