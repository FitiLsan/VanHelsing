using DialogueSystem;
using UnityEngine;


namespace BeastHunter {
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/StartDialogue", order = 0)]
    public sealed class StartDialogueData : ScriptableObject
    {
        #region Fields

        public StartDialogueStruct StartDialogueStruct;
        public StartDialogueModel Model;
        public Vector3 npcPos;   
        public int _npcID;
        public DialogueSystemModel dialogueSystemModel;
        public GameObject canvasNpc;

        #endregion


        #region Events

        public delegate void DialogueView(bool isShow);
        public static event DialogueView ShowCanvasEvent;

        #endregion

   
        #region Metods

        public void DialogUsing()
        {
            if (Model.isDialogueAreaEnter)
            {
                if (!dialogueSystemModel.dialogueCanvas.enabled)
                {
                    canvasNpc.SetActive(true);
                    canvasNpc.transform.LookAt(Camera.main.transform);
                }
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
                    if (dialogueSystemModel.dialogueCanvas.enabled)
                    {
                        DialogStatus(false);
                    }
                }
            }
        }

        private void DialogStatus(bool isShowDialogCanvas)
        {
            ShowCanvasEvent?.Invoke(isShowDialogCanvas);
        }

        public void DialogAreaEnterSwitcher(bool isOn)
        {
            Model.isDialogueAreaEnter = isOn;
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

        public void OnTriggerEnter(Collider other)
        {
                dialogueSystemModel = Model._context._dialogueSystemModel;
                var getNpcInfo = other.GetComponent<IGetNpcInfo>().GetInfo();
                _npcID = getNpcInfo.Item1;
                npcPos = getNpcInfo.Item2;
                canvasNpc.transform.position = new Vector3(npcPos.x, npcPos.y + GetCanvasOffset(), npcPos.z);
                DialogAreaEnterSwitcher(true);
                dialogueSystemModel.npcID = _npcID;
                dialogueSystemModel.dialogueNode = DialogueGenerate.DialogueCreate(_npcID);
        }

        public void OnTriggerExit(Collider other)
        {
            DialogAreaEnterSwitcher(false);
        }

        public void GetDialogueSystemModel(DialogueSystemModel model)
        {
            dialogueSystemModel = model;
        }

        #endregion
    }
}
