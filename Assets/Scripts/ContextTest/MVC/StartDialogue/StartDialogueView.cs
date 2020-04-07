//using UnityEngine;


//namespace BeastHunter
//{
//    public class StartDialogueView : MonoBehaviour
//    {
//        //        [SerializeField]
//        //        public GameObject canvasPref;
//        //        [SerializeField]
//        //        public Text text;
//        //        public Vector3 npcPos;
//        //        public GameObject canvasNPC;
//        //        public int _npcID;
//        //        public DialogueSystemView dialogueSystemView;
//        //        public DialogueSystemModel dialogueSystemModel;
//        public StartDialogueModel Model;

//        private void Start()
//        {
            
//            //            dialogueSystemView = FindObjectOfType<DialogueSystemView>();
//            //            canvasNPC = GameObject.FindGameObjectWithTag("DialogueNpcGUI");
//            //            canvasNPC.SetActive(false);
//            //            text = canvasNPC.transform.Find("Image").gameObject.GetComponentInChildren<Text>();
//            //            text.text = "\"E\" Начать диалог";
//        }

//        public void OnTriggerEnter(Collider other)
//        {

//            if (other.transform.tag == "NPC")
//            {
//                Debug.Log("enter");
//                //dialogueSystemModel = Model._context._dialogueSystemModel;
//               // var getNpcInfo = other.GetComponent<IGetNpcInfo>().GetInfo();
//                //_npcID = getNpcInfo.Item1;
//                //npcPos = getNpcInfo.Item2;
//                //canvasNPC.transform.position = new Vector3(npcPos.x, npcPos.y + Model.canvasOffset, npcPos.z);//Controller.GetCanvasOffset(), npcPos.z);
//                Model.dialogAreaEnter = true;
//                ////   Controller.DialogAreaEnterSwitcher(true);
//                //dialogueSystemModel.npcID = _npcID;
//                //dialogueSystemModel.dialogueNode = DialogueGenerate.DialogueCreate(_npcID);
//            }
//        }

//        public void OnTriggerExit(Collider other)
//        {
//            if (other.transform.tag == "NPC")
//            {
//                Debug.Log("exit");
//                Model.dialogAreaEnter = false;
//                //   Controller.DialogAreaEnterSwitcher(false);
//            }
//        }

//        public void GetModel(StartDialogueModel model)
//        {
//            Model = model;
//        }
//    }

//}
