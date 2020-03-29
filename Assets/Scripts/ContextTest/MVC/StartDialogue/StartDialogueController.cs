using UnityEngine;

namespace BeastHunter
{
    public sealed class StartDialogueController : IUpdate
    {
        #region Fields
        private readonly GameContext _context;
        #endregion
        #region Properties
        public StartDialogueModel Model { get; private set; }
        public StartDialogueView View { get; private set; }
        #endregion
        #region ClassLifeCycle
        public StartDialogueController(GameContext context, Services services)
        {
            _context = context;   
        }
        #endregion
        #region Updating

        public void Updating()
        {
            if (Model == null)
            {
                Model = _context._startDialogueModel;
            }

            if (View == null)
            {
                View = GameObject.FindObjectOfType<StartDialogueView>();
                View.GetModel(Model);
                _context._startDialogueModel.Initilize();
                SetPerent();
            }
           

            DialogUsing();
        }

        #endregion
 
        public delegate void DialogueView(bool isShow);  // вынести в EventManager?
        public static event DialogueView ShowCanvasEvent;

        #region Methods

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
        public void SetPerent()
        {
            View.transform.parent = _context._startDialogueModel.parentTransform;
            View.transform.position = _context._startDialogueModel.parentTransform.transform.position;
        }
        #endregion
    }
}
