using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class DialogueSystemModel
    {
        #region Properties

        public Transform DialogueSystemTransform{ get; }
        public DialogueSystemData DialogueSystemData { get; }
        public DialogueSystemStruct DialogueSystemStruct { get; }

        public Button[] answerButtons { get; }
        public Canvas dialogueCanvas { get; }
        public Text dialogueNPCText { get; }

        #endregion


        #region Fields

        public int currentNode;
        public int npcID;
        public List<Dialogue> dialogueNode;

        #endregion


        #region ClassLifeCycle

        public DialogueSystemModel(GameObject prefab, DialogueSystemData dialogueSystemData)
        {
            DialogueSystemData = dialogueSystemData;
            DialogueSystemStruct = dialogueSystemData.DialogueSystemStruct;
            DialogueSystemTransform = prefab.transform;
            dialogueSystemData.Model = this;
            dialogueCanvas = prefab.GetComponentInChildren<Canvas>();
            dialogueNPCText = prefab.GetComponentInChildren<Text>();
            answerButtons = prefab.GetComponentsInChildren<Button>();
            dialogueCanvas.enabled = false;

            ButtonClick.MouseClickEvent += dialogueSystemData.SelectAnswer;
            ButtonClick.KeyBoardButtonDownEvent += dialogueSystemData.ButtonClickNumber;
            StartDialogueData.ShowCanvasEvent += dialogueSystemData.CanvasSwitcher;
        }

        #endregion


        #region Metods

        public void Initilize()
        {
        }

        #endregion
    }
}