using UnityEngine;


namespace BeastHunter
{
    public class StartDialogueModel
    {
        #region Fields

        public GameContext _context;
        public DialogueSystemModel dialogueSystemModel;

        public float canvasOffset = 1.5f;
        public bool isStartDialogueFlagOn;
        public bool isDialogueAreaEnter;
        
        #endregion


        #region Properties

        public Transform StartDialogueTransform;
        public StartDialogueData StartDialogueData;
        public StartDialogueStruct StartDialogueStruct;

        #endregion


        #region ClassLifeCycle

        public StartDialogueModel(GameObject prefab,GameObject canvasNpc, StartDialogueData startDialogueData, GameContext context)
        {
            StartDialogueTransform = prefab.transform;
            StartDialogueData = startDialogueData;
            StartDialogueStruct = startDialogueData.StartDialogueStruct;
            _context = context;
            startDialogueData.Model = this;
            startDialogueData.canvasNpc = canvasNpc;
        }

        #endregion


        #region Metods

        public void Initilize()
        {
            StartDialogueData.DialogUsing();            
        }

        #endregion
    }
}
