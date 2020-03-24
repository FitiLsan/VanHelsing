using BaseScripts;
using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystemView : MonoBehaviour
{
    [SerializeField]
    public Button[] answerButtons;
    [SerializeField]
    public Canvas dialogueCanvas;
    [SerializeField]
    public Text dialogueNPCText;
    [SerializeField]
    public GameObject dialogueSystemViewPrefab;
    [SerializeField]
    public GameObject dialogueSystemView;

    public DialogueSystemController Controller { get; private set; }
    private void Awake()
    {
        if (dialogueSystemView == null)
        {
            dialogueSystemView = GameObject.FindGameObjectWithTag("DialogueSystemView");

            if (dialogueSystemView == null)
            {
                dialogueSystemView = Instantiate(dialogueSystemViewPrefab);
            }
        }

        dialogueCanvas = gameObject.GetComponentInChildren<Canvas>();
        dialogueNPCText = gameObject.GetComponentInChildren<Text>();
        answerButtons = gameObject.GetComponentsInChildren<Button>();

    }
    private void Start()
    {
        dialogueCanvas.enabled = false;
        Controller = StartScript.GetStartScript.DialogueSystemController;
        Controller.GetView(this);
    }
}
