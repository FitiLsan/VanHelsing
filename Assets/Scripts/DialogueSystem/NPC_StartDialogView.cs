using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem

{
    public class NPC_StartDialogView : MonoBehaviour
    {
        [SerializeField] private Transform _canvasNPC;

        [SerializeField] private GameObject _dialogPanel;

        public int _npcID;
        private Vector3 _npcPos;
        public bool _startDialogFlag;

        [SerializeField] private Text _text;

        private DialogueSystem dialogueSystem;

        private void Awake()
        {
            _canvasNPC = GameObject.Find("DialogueNpcGUI").transform;
            _dialogPanel = _canvasNPC.Find("Image").gameObject;
            _text = _dialogPanel.GetComponentInChildren<Text>();

            dialogueSystem = FindObjectOfType<DialogueSystem>();
            _npcPos = gameObject.transform.position;
        }

        private void Start()
        {
            _dialogPanel.SetActive(false);
            _text.text = "\"E\" Начать диалог";
        }


        private void Update()
        {
            _canvasNPC.LookAt(Camera.main.transform);


            if (Input.GetButton("Use") & dialogueSystem._dialogAreaEnter)
            {
                dialogueSystem.gameObject.GetComponentInChildren<Canvas>().enabled = true;
                _startDialogFlag = true;
            }

            if (Input.GetButton("Cancel") || dialogueSystem._dialogAreaEnter == false)
            {
                dialogueSystem.gameObject.GetComponentInChildren<Canvas>().enabled = false;
                dialogueSystem._currentNode = 0;
                _startDialogFlag = false;
            }

            ShowDialogueGUI();
        }

        private void ShowDialogueGUI()
        {
            if ((_startDialogFlag == false) & dialogueSystem._dialogAreaEnter)
                _dialogPanel.SetActive(true);
            else
                _dialogPanel.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                _canvasNPC.transform.position = new Vector3(_npcPos.x, _npcPos.y + 1.5f, _npcPos.z);
                dialogueSystem._dialogAreaEnter = true;
                dialogueSystem._dialogueReady = true;
                dialogueSystem._npcID = _npcID;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                //_dialogPanel.SetActive(false);
                dialogueSystem._dialogAreaEnter = false;
                dialogueSystem._dialogueReady = false;
            }
        }
    }
}