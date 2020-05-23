using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public sealed class DialogueSystemModel
    {
        #region Fields

        public List<Dialogue> DialogueNode;
        public int CurrentNode;
        public int NpcID;

        #endregion


        #region Properties

        public Transform DialogueSystemTransform{ get; }
        public DialogueSystemData DialogueSystemData { get; }
        public DialogueSystemStruct DialogueSystemStruct { get; }

        public Button[] AnswerButtons { get; }
        public Canvas DialogueCanvas { get; }
        public Text DialogueNPCText { get; }

        #endregion


        #region ClassLifeCycle

        public DialogueSystemModel(GameObject prefab, DialogueSystemData dialogueSystemData)
        {
            DialogueSystemData = dialogueSystemData;
            DialogueSystemStruct = dialogueSystemData.DialogueSystemStruct;
            DialogueSystemTransform = prefab.transform;
            dialogueSystemData.Model = this;
            DialogueCanvas = prefab.GetComponentInChildren<Canvas>();
            DialogueNPCText = prefab.GetComponentInChildren<Text>();
            AnswerButtons = prefab.GetComponentsInChildren<Button>();
            DialogueCanvas.enabled = false;

            Services.SharedInstance.EventManager.StartListening(GameEventTypes.AnswerButtonClicked, dialogueSystemData.SelectAnswerById);
            ButtonClick.KeyBoardButtonDownEvent += dialogueSystemData.ButtonClickNumber;
            StartDialogueData.ShowCanvasEvent += dialogueSystemData.CanvasSwitcher;
        }

        #endregion


        #region Metods

        public void Execute()
        {
        }

        #endregion
    }
}