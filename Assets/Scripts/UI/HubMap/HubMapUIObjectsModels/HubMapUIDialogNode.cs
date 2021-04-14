using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class HubMapUIDialogNode
    {
        #region Fields

        [SerializeField, ReadOnlyInUnityInspector] private int _id;
        [SerializeField][TextArea(3, 10)] private string _text;
        [SerializeField] private List<HubMapUIDialogAnswer> _answers;

        #endregion


        #region Properties

        public int Id => _id;
        public string Text => _text;
        public List<HubMapUIDialogAnswer> Answers => _answers;

        #endregion


        #region ClassLifeCycle

        public HubMapUIDialogNode(HubMapUIDialogNode hubMapUIDialogNode)
        {
            _id = hubMapUIDialogNode.Id;
            _text = hubMapUIDialogNode.Text;
            _answers = new List<HubMapUIDialogAnswer>();

            for (int i = 0; i < hubMapUIDialogNode.Answers.Count; i++)
            {
                _answers.Add(new HubMapUIDialogAnswer(hubMapUIDialogNode.Answers[i]));
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
