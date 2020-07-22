using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/StartDialogue", order = 0)]
    public sealed class StartDialogueData : ScriptableObject
    {
        #region Fields
        public const float CANVAS_OFFSET = 2f;

        public StartDialogueStruct StartDialogueStruct;
        public StartDialogueModel Model;
        public Vector3 NpcPos;
        public int NpcID;
        public DialogueSystemModel DialogueSystemModel;
        public GameObject CanvasNpc;
        public Transform CharacterCamera;
        public bool dialogueStatus;

        #endregion


        #region Events

        public static event Action<bool> ShowCanvasEvent;

        #endregion


        #region Metods

        public void DialogUsing()
        {
            if (Model.IsDialogueAreaEnter)
            {
                if (!DialogueSystemModel.DialogueCanvas.enabled)
                {
                   // CanvasNpc.SetActive(true);//For 3d mode
                    // CanvasNpc.transform.LookAt(GetCharacterCamera()); //For 3d mode
                }
                if (Input.GetButtonDown("Use"))
                {
                    DialogStatus(true);
                    // CanvasNpc.SetActive(false); //For 3d mode
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    DialogStatus(false);
                    // CanvasNpc.SetActive(true);//For 3d mode
                }
            }
            else
            {
                if (Model != null)
                {
                  //  CanvasNpc.SetActive(false);//For 3d mode
                    if (DialogueSystemModel.DialogueCanvas.enabled)
                    {
                        DialogStatus(false);
                    }
                }
            }
        }

        public void OnDialogueStart(int npcId)
        {
            NpcID = npcId;
            if (NpcID == 0)
            {
                DialogStatus(true);
            }
            DialogueSwitcher();

        }

        private void DialogueSwitcher()
        {
            if (!dialogueStatus)
            {
                DialogStatus(true);
            }
            else
            {
                DialogStatus(false);
                NpcID = 0;
            }
        }

        public void OnUpdateDialogueByQuest(EventArgs args)// test updating in dialog
        {
            DialogStatus(true);
        }

        private void DialogStatus(bool isShowDialogCanvas)
        {
            DialogueSystemModel.DialogueNode = DialogueGenerate.DialogueCreate(NpcID, Model.Context);
            ShowCanvasEvent?.Invoke(isShowDialogCanvas);
            dialogueStatus = isShowDialogCanvas;
        }

        public void DialogAreaEnterSwitcher(bool isOn = true)// for MAP True 
        {
            Model.IsDialogueAreaEnter = isOn;
        }

        //public Transform GetParent()
        //{
        //    var player = GameObject.FindGameObjectWithTag(TagManager.PLAYER);  //For 3d mode
        //    return player.transform;
        //}

        public void SetPerent(Transform startDialogueTransform, Transform parent)
        {
            startDialogueTransform.parent = parent;
            startDialogueTransform.position = parent.position;
        }

        public void OnTriggerEnter(Collider other)
        {
            DialogueSystemModel = Model.Context.DialogueSystemModel;
            var getNpcInfo = other.GetComponent<IGetNpcInfo>().GetInfo();
            NpcID = getNpcInfo.Item1;
            NpcPos = getNpcInfo.Item2;
            //  CanvasNpc.transform.position = new Vector3(NpcPos.x, NpcPos.y + CANVAS_OFFSET, NpcPos.z); //For 3d mode
            //  DialogAreaEnterSwitcher(true); //For 3d mode
            DialogueSystemModel.NpcID = NpcID;
        }

        public void OnTriggerExit(Collider other)
        {
            // DialogAreaEnterSwitcher(false); //For 3d mode
        }

        public void GetDialogueSystemModel(DialogueSystemModel model)
        {
            DialogueSystemModel = model;
        }

        //public Transform GetCharacterCamera() //For 3d mode
        //{
        //  // return Services.SharedInstance.CameraService.CharacterCamera.transform; //For 3d mode
        //}
        #endregion
    }
}
