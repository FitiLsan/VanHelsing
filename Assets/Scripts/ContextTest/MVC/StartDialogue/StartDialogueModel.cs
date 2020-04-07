using UnityEngine;

namespace BeastHunter
{


    public class StartDialogueModel
    {
        #region Fields
        public GameContext _context;

        public GameObject canvasNpc;
        public DialogueSystemView dialogueSystemView;
        public DialogueSystemModel dialogueSystemModel;


        public float canvasOffset = 1.5f;
        public bool _startDialogFlag;
        public bool dialogAreaEnter;
        
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
            this.canvasNpc = canvasNpc;
        }

        #endregion
        #region Metods

        public void Initilize()
        {
            StartDialogueData.DialogUsing(this);            
        }
        #endregion
    }
}
