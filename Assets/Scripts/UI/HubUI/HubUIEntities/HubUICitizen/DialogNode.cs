using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class DialogNode
    {
        #region Fields

        [SerializeField, ReadOnlyInUnityInspector] private int _id;
        [SerializeField][TextArea(3, 10)] private string _text;
        [SerializeField] private List<DialogAnswer> _answers;

        #endregion


        #region Properties

        public int Id => _id;
        public string Text => _text;
        public List<DialogAnswer> Answers => _answers;

        #endregion


        #region ClassLifeCycle

        public DialogNode(DialogNode hubMapUIDialogNode)
        {
            _id = hubMapUIDialogNode.Id;
            _text = hubMapUIDialogNode.Text;
            _answers = new List<DialogAnswer>();

            for (int i = 0; i < hubMapUIDialogNode.Answers.Count; i++)
            {
                _answers.Add(new DialogAnswer(hubMapUIDialogNode.Answers[i]));
            }
        }

        #endregion


        #region Methods

        public void SetId(int id)
        {
            _id = id;
        }

        #endregion
    }
}
