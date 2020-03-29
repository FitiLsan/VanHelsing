using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class DialogueSystemModel
    {
        #region Properties

        public Transform DialogueSystemTransform;// { get; }
        public DialogueSystemData DialogueSystemData;
        public DialogueSystemStruct DialogueSystemStruct;
        public Transform parentTransform { get; private set; }

        #endregion
        public int currentNode;
        public bool isDialogueReady;
        public int npcID { get; set; }
        [SerializeField]
        public List<Dialogue> dialogueNode;

        #region ClassLifeCycle

        public DialogueSystemModel(GameObject prefab, DialogueSystemData dialogueSystemData)
        {
            DialogueSystemData = dialogueSystemData;
            DialogueSystemStruct = dialogueSystemData.DialogueSystemStruct;
            DialogueSystemTransform = prefab.transform;
        }

        #endregion
        #region Metods

        public void Initilize()
        {
        }

        #endregion
    }
}