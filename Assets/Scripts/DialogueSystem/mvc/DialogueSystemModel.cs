using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemModel
{
   // [SerializeField] public Button[] answerButtons;
   // [SerializeField] public Canvas dialogueCanvas;
   [SerializeField] public List<Dialogue> dialogueNode;
   // [SerializeField] public Text dialogueNPCText;

    public int currentNode;
    public bool isDialogueReady;
    public int npcID { get; set; }
}
