using BaseScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class StartDialogueController : BaseController
    {
        public StartDialogueModel Model { get; private set; }
        public StartDialogueView View { get; private set; }

        public delegate void DialogueView(bool isShow);  // вынести в EventManager?
        public static event DialogueView ShowCanvasEvent;

        public StartDialogueController()
        {
            Model = new StartDialogueModel();
        }

        //private void Start()
        //{
        //    View.canvasNPC.SetActive(false);
        //    View.text.text = "\"E\" Начать диалог";
        //}
        public override void ControllerUpdate()
        {
            DialogUsing();
        }

        private void DialogUsing()
        {
            if (Model.dialogAreaEnter)
            {
                if (!View.dialogueSystemView.dialogueCanvas.enabled)
                {
                    View.canvasNPC.SetActive(true);
                    View.canvasNPC.transform.LookAt(Camera.main.transform);
                }
                if (Input.GetButton("Use"))
                {
                    DialogStatus(true);
                    View.canvasNPC.SetActive(false);
                }
                if (Input.GetButton("Cancel"))
                {
                    DialogStatus(false);
                    View.canvasNPC.SetActive(true);
                }
            }
            else
            {
                if (View != null)
                {
                    View.canvasNPC.SetActive(false);
                    if (View.dialogueSystemView.dialogueCanvas.enabled)
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
            Model.dialogAreaEnter = isOn;
        }

        public float GetCanvasOffset()
        {
            return Model.canvasOffset;
        }
        public void GetView(StartDialogueView view)
        {
            View = view;
        }
    }
}
