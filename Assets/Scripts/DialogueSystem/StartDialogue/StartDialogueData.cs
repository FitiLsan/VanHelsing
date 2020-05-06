using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/StartDialogue", order = 0)]
    public sealed class StartDialogueData : ScriptableObject
    {
        #region Fields
        public const float CANVAS_OFFSET = 2.3f;

        public StartDialogueStruct StartDialogueStruct;
        public StartDialogueModel Model;
        public Vector3 NpcPos;
        public int NpcID;
        public DialogueSystemModel DialogueSystemModel;
        public GameObject CanvasNpc;
        public Transform CharacterCamera;

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
                    CanvasNpc.SetActive(true);
                    CanvasNpc.transform.LookAt(GetCharacterCamera());
                }
                if (Input.GetButton("Use"))
                {
                    DialogStatus(true);
                    CanvasNpc.SetActive(false);
                }
                if (Input.GetButton("Cancel"))
                {
                    DialogStatus(false);
                    CanvasNpc.SetActive(true);
                }
            }
            else
            {
                if (Model != null)
                {
                    CanvasNpc.SetActive(false);
                    if (DialogueSystemModel.DialogueCanvas.enabled)
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
            Model.IsDialogueAreaEnter = isOn;
        }

        public Transform GetParent()
        {
            var player = GameObject.FindGameObjectWithTag(TagManager.PLAYER);
            return player.transform;
        }

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
            CanvasNpc.transform.position = new Vector3(NpcPos.x, NpcPos.y + CANVAS_OFFSET, NpcPos.z);
            DialogAreaEnterSwitcher(true);
            DialogueSystemModel.NpcID = NpcID;
            DialogueSystemModel.DialogueNode = DialogueGenerate.DialogueCreate(NpcID, Model.Context);
        }

        public void OnTriggerExit(Collider other)
        {
            DialogAreaEnterSwitcher(false);
        }

        public void GetDialogueSystemModel(DialogueSystemModel model)
        {
            DialogueSystemModel = model;
        }

        public Transform GetCharacterCamera()
        {
           return Model.Context.CharacterModel.CharacterCamera.transform;
        }
        #endregion
    }
}
