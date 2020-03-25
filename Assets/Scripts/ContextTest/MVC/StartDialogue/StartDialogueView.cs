using BaseScripts;
using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class StartDialogueView : MonoBehaviour
    {
        [SerializeField]
        public GameObject canvasPref;
        [SerializeField]
        public Text text;
        public Vector3 npcPos;
        public GameObject canvasNPC;
        //   public StartDialogueController Controller { get;  private set; }
        public int _npcID;
        public DialogueSystemView dialogueSystemView;
        public DialogueSystemModel dialogueSystemModel;
        public StartDialogueModel Model;
        

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


        }

        private void Start()
        {
      
            //  Controller = StartScript.GetStartScript.StartDialogueController;
            canvasNPC.SetActive(false);
            text.text = "\"E\" Начать диалог";
            dialogueSystemModel = StartScript.GetStartScript.DialogueSystemController.Model;
            //Model = GameObject.FindGameObjectWithTag("");
        }

        public void OnTriggerEnter(Collider other)
        {
           
            if (other.transform.tag == "NPC")
            {
                var getNpcInfo = other.GetComponent<IGetNpcInfo>().GetInfo();
                _npcID = getNpcInfo.Item1;
                npcPos = getNpcInfo.Item2;
                // Controller.GetView(this);
                canvasNPC.transform.position = new Vector3(npcPos.x, npcPos.y + 1.5f, npcPos.z);//Controller.GetCanvasOffset(), npcPos.z);
                Model.dialogAreaEnter = true;

                dialogueSystemModel.npcID = _npcID; 
                dialogueSystemModel.dialogueNode = DialogueGenerate.DialogueCreate(_npcID); 
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "NPC")
            {
                Model.dialogAreaEnter = false;
                //   Controller.DialogAreaEnterSwitcher(false);
            }
        }

        public void GetModel(StartDialogueModel model)
        {
            Model = model;
        }
    }

}
