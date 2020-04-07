using DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter {
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/StartDialogue", order = 0)]
    public sealed class StartDialogueData : ScriptableObject
    {
        #region Fields

        public StartDialogueStruct StartDialogueStruct;
        public StartDialogueModel Model;
        //=================View==//
        [SerializeField]
        public GameObject canvasPref;
        [SerializeField]
        public Text text;
        public Vector3 npcPos;   
        public int _npcID;
        public DialogueSystemView dialogueSystemView;
        public DialogueSystemModel dialogueSystemModel;
        public GameObject canvasNpc;
        #endregion

        #region Metods

        #region Events
        public delegate void DialogueView(bool isShow);  // вынести в EventManager?
        public static event DialogueView ShowCanvasEvent;
        #endregion


      
        public void DialogUsing(StartDialogueModel Model)
        {
            this.Model = Model;
            canvasNpc = Model.canvasNpc;
            if (Model.dialogAreaEnter)
            {
               // if (!dialogueSystemView.dialogueCanvas.enabled)
              //  {
                    canvasNpc.SetActive(true);
                    canvasNpc.transform.LookAt(Camera.main.transform);
              //  }
                if (Input.GetButton("Use"))
                {
                    DialogStatus(true);
                    canvasNpc.SetActive(false);
                }
                if (Input.GetButton("Cancel"))
                {
                    DialogStatus(false);
                    canvasNpc.SetActive(true);
                }
            }
            else
            {
                if (Model != null)
                {
                    canvasNpc.SetActive(false);
                   // if (dialogueSystemView.dialogueCanvas.enabled)
                 //   {
                        DialogStatus(false);
                  //  }
                }
            }
        }

        private void DialogStatus(bool isShowDialogCanvas)
        {
            ShowCanvasEvent?.Invoke(isShowDialogCanvas);
        }

        public void DialogAreaEnterSwitcher(bool isOn)
        {
            Model.dialogAreaEnter = isOn;
        }

        public float GetCanvasOffset()
        {
            return Model.canvasOffset;
        }

        public Transform GetParent()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.transform;
        }

        public void SetPerent(Transform startDialogueTransform, Transform parent)
        {
            startDialogueTransform.parent = parent;
            startDialogueTransform.position = parent.position;
        }

        //====View=====//
        
        //public void OnTriggerEnter(Collider other)
        //{

        //    if (other.transform.tag == "NPC")
        //    {
        //        Debug.Log("enter into npc");
        //        dialogueSystemModel = Model._context._dialogueSystemModel;
        //        //var getNpcInfo = other.GetComponent<IGetNpcInfo>().GetInfo();
        //        //_npcID = getNpcInfo.Item1;
        //        //npcPos = getNpcInfo.Item2;
        //        //canvasNpc.transform.position = new Vector3(npcPos.x, npcPos.y + Model.canvasOffset, npcPos.z);//Controller.GetCanvasOffset(), npcPos.z);
        //        //Model.dialogAreaEnter = true;
        //       // //   Controller.DialogAreaEnterSwitcher(true);
        //        dialogueSystemModel.npcID = _npcID;
        //        dialogueSystemModel.dialogueNode = DialogueGenerate.DialogueCreate(_npcID);
        //    }
        //}

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.transform.tag == "NPC") Debug.Log("Exit from npc");
        //    //{
        //    //    Model.dialogAreaEnter = false;
        //    //   // //   Controller.DialogAreaEnterSwitcher(false);
        //    //}
        //}

        public void GetDialogueSystemModel(DialogueSystemModel model)
        {
            dialogueSystemModel = model;
            dialogueSystemView = FindObjectOfType<DialogueSystemView>();
        }
        #endregion
    }
}
