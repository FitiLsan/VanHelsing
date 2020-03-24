using BaseScripts;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class StartDialogueView : MonoBehaviour
    {
        [SerializeField]
        public GameObject canvasPref;
        [SerializeField]
        public Text text;
        public Vector3 npcPos;
        public GameObject canvasNPC;
        public StartDialogueController Controller { get;  private set; }
        public int _npcID; // в модель, но пока назначается в редакторе
        //public DialogueSystem dialogueSystem;
        public DialogueSystemView dialogueSystemView;
        public DialogueSystemModel dialogueSystemModel;



        private void Awake()
        {
            if (canvasNPC == null)
            {
                canvasNPC = GameObject.FindGameObjectWithTag("DialogueNpcGUI");

                if (canvasNPC == null)
                {
                    canvasNPC = Instantiate(canvasPref);
                }
            }

            dialogueSystemView = FindObjectOfType<DialogueSystemView>();
            text = canvasNPC.transform.Find("Image").gameObject.GetComponentInChildren<Text>();
            npcPos = gameObject.transform.position;
            
        }

        private void Start()
        {
           // Controller = StartScript.GetStartScript.StartDialogueController;
            dialogueSystemModel = StartScript.GetStartScript.DialogueSystemController.Model;
            canvasNPC.SetActive(false);
            text.text = "\"E\" Начать диалог";
        }

        public void OnTriggerEnter(Collider other)
        {
           
            if (other.transform.tag == "Player")
            {
                Controller.GetView(this);
                canvasNPC.transform.position = new Vector3(npcPos.x, npcPos.y + Controller.GetCanvasOffset(), npcPos.z);
                Controller.DialogAreaEnterSwitcher(true);
                dialogueSystemModel.npcID = _npcID;
                dialogueSystemModel.dialogueNode = DialogueGenerate.DialogueCreate(_npcID);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                Controller.DialogAreaEnterSwitcher(false);
            }
        }
    }

}
