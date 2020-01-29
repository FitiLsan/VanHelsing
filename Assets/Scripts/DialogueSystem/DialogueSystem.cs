using System;
using System.Collections.Generic;
using BaseScripts;
using Controllers;
using Events;
using Events.Args;
using Extensions;
using Quests;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem

{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField] private Button[] _AnswerButtons;

        public int _currentNode;
        public bool _dialogAreaEnter;

        [SerializeField] private Canvas _dialogueCanvas;

        [SerializeField] private Text _dialogueNPCText;

        [SerializeField] private Image _dialoguePanel;

        public bool _dialogueReady;

        public int _npcID { get; set; }
        private Dialogue dialogue;//

        [SerializeField] private List<Dialogue> dialogueNode;//

        private PlayerAnswer playerAnswer;//
        private bool _previousState = false;


        private void Awake()
        {
            _dialogueCanvas = gameObject.GetComponentInChildren<Canvas>();
            _dialoguePanel = gameObject.GetComponentInChildren<Image>();
            _dialogueNPCText = gameObject.GetComponentInChildren<Text>();
            _AnswerButtons = gameObject.GetComponentsInChildren<Button>();
        }

        private void Start()
        {
            DialogueAnswerClear();
            _dialogueCanvas.enabled = false;
            _previousState = _dialogueCanvas.enabled;
        }

        private void Update()
        {
            DialogueCreate();
            DialogueUpdate();
            ButtonClickNumber();
            LockCharAction();
        }


        public void DialogueAnswerClear()
        {
            foreach (var b in _AnswerButtons)
            {
                b.gameObject.SetActive(false);
                
            }
        }

        private void DialogueUpdate()
        {
            if (_dialogueReady)
            {
                _dialogueNPCText.text = dialogueNode[_currentNode]._npcText;

                for (var i = 0; i < dialogueNode[_currentNode].playerAnswers.Count; i++)
                {
                    var _buttonText = _AnswerButtons[i].GetComponentInChildren<Text>();

                    _AnswerButtons[i].gameObject.SetActive(true);
                    _buttonText.color = Color.black;
                    _buttonText.name = dialogueNode[_currentNode].playerAnswers[i]._text;
                    _buttonText.text = dialogueNode[_currentNode].playerAnswers[i]._text;

                    if (dialogueNode[_currentNode].playerAnswers[i]._isStartQuest)
                    {
                        _buttonText.color = Color.yellow;
                        _buttonText.text += "(квест)";

                    }
                    if (dialogueNode[_currentNode].playerAnswers[i]._isEndQuest )
                    {
                        _buttonText.color = Color.red;
                        _buttonText.text += "(квест)";
                    }
                    if (dialogueNode[_currentNode].playerAnswers[i]._end )
                    {
                        _buttonText.text += "(выход)";
                    }

                }

                for (var i = dialogueNode[_currentNode].playerAnswers.Count; i < _AnswerButtons.Length; i++)
                    _AnswerButtons[i].gameObject.SetActive(false);
            }
        }

        private void DialogueCreate()
        {
            if (_dialogueReady)
            {
                dialogueNode = new List<Dialogue>();

                var npc_dt = DatabaseWrapper.DatabaseWrapper.GetTable($"select * from 'dialogue_node' where Npc_id={_npcID};");


                for (var j = 0; j < npc_dt.Rows.Count; j++)
                {
                    var npc_text = npc_dt.Rows[j].GetString(3);

                    dialogueNode.Add(new Dialogue(npc_text, new List<PlayerAnswer>()));

                    var answer_dt = DatabaseWrapper.DatabaseWrapper.GetTable($"select * from 'dialogue_answers' where Node_id={j} and Npc_id={_npcID};");

                    for (var i = 0; i < answer_dt.Rows.Count; i++)
                    {
                        var answer_id = answer_dt.Rows[i].GetInt(0);
                        var answer_text = answer_dt.Rows[i].GetString(2);
                        var answer_toNode = answer_dt.Rows[i].GetInt(3);
                        var answer_endDialogue = answer_dt.Rows[i].GetInt(4);
                        var answer_isStartQuest = answer_dt.Rows[i].GetInt(6);
                        var answer_isEndQuest = answer_dt.Rows[i].GetInt(7);
                        var answer_questId = answer_dt.Rows[i].GetInt(8);
                        dialogueNode[j].playerAnswers.Add(new PlayerAnswer(answer_id, answer_text, answer_toNode, answer_endDialogue,answer_isStartQuest,answer_isEndQuest, answer_questId));
                    }
                }
            }
            else
            {
                DialogueAnswerClear();
            }
        }

        public void ButtonClickCheck(int buttonNumber)
        {
            {
                EventManager.TriggerEvent(GameEventTypes.DialogAnswerSelect, new DialogArgs(dialogueNode[_currentNode].playerAnswers[buttonNumber]._answerId, _npcID));
                Debug.Log($"id answer {dialogueNode[_currentNode].playerAnswers[buttonNumber]._answerId}, id npc  {_npcID}");
                

                if (dialogueNode[_currentNode].playerAnswers[buttonNumber]._isStartQuest)
                {
                  
                   EventManager.TriggerEvent(GameEventTypes.QuestAccepted, new IdArgs(dialogueNode[_currentNode].playerAnswers[buttonNumber]._questId));
                }

                if (dialogueNode[_currentNode].playerAnswers[buttonNumber]._isEndQuest)
                {
                    EventManager.TriggerEvent(GameEventTypes.QuestReported , new IdArgs(dialogueNode[_currentNode].playerAnswers[buttonNumber]._questId));
                }

                if (dialogueNode[_currentNode].playerAnswers[buttonNumber]._end)
                {
                    _dialogueCanvas.enabled = false;
                    _currentNode = 0;  
                    return;
                }
                _currentNode = dialogueNode[_currentNode].playerAnswers[buttonNumber]._toNode;
            }
        }

        public void ButtonClickNumber()
        {
            if (_dialogueCanvas.enabled)
                for (var i = 0; i < _AnswerButtons.Length; i++)
                    if (Input.GetKeyDown($"{i + 1}"))
                    {
                        if (i < dialogueNode[_currentNode].playerAnswers.Count)
                            ButtonClickCheck(i);
                        else
                            return;
                    }
        }

        public void LockCharAction()
        {
            if (_previousState == _dialogueCanvas.enabled) return;
            _previousState = _dialogueCanvas.enabled;
            if (_previousState)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                StartScript.GetStartScript.InputController.Off();
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                StartScript.GetStartScript.InputController.On();
            }
        }
    }
}