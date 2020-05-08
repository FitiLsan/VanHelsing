using UnityEngine;


namespace BeastHunter
{
    public sealed class StartDialogueModel
    {
        #region Fields

        public GameContext Context;
        public DialogueSystemModel DialogueSystemModel;
        public bool IsStartDialogueFlagOn;
        public bool IsDialogueAreaEnter;

        #endregion


        #region Properties

        public Transform StartDialogueTransform { get; }
        public StartDialogueData StartDialogueData { get; }
        public StartDialogueStruct StartDialogueStruct { get; }

        #endregion


        #region ClassLifeCycle

        public StartDialogueModel(GameObject prefab, GameObject canvasNpc, StartDialogueData startDialogueData, GameContext context)
        {
            StartDialogueTransform = prefab.transform;
            StartDialogueData = startDialogueData;
            StartDialogueStruct = startDialogueData.StartDialogueStruct;
            Context = context;
            startDialogueData.Model = this;
            startDialogueData.CanvasNpc = canvasNpc;
        }

        #endregion


        #region Metods

        public void Execute()
        {
            StartDialogueData.DialogUsing();            
        }

        #endregion
    }
}
