using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HubMapUICitizenModel
    {
        #region Fields

        private HubMapUIQuestMarkerType _questMarkerType;

        #endregion


        #region Properties

        public Action<HubMapUIQuestMarkerType> OnChangeQuestMarkerTypeHandler { get; set; }

        public int DataInstanceId { get; private set; }
        public string Name { get; private set; }
        public Sprite Portrait { get; private set; }
        public List<HubMapUIQuestAnswer> QuestAnswers { get; set; }
        public List<HubMapUIDialogNode> Dialogs { get; private set; }
        public HubMapUIDialogNode CurrentDialog { get; private set; }

        public HubMapUIQuestMarkerType QuestMarkerType
        { 
            get
            {
                return _questMarkerType;
            }
            set
            {
                if (value != _questMarkerType)
                {
                    _questMarkerType = value;
                    OnChangeQuestMarkerTypeHandler?.Invoke(_questMarkerType);
                }
            }
        }

        #endregion


        #region ClassLifeCyrcle

        public HubMapUICitizenModel(HubMapUICitizenData data)
        {
            DataInstanceId = data.GetInstanceID();
            Name = data.Name;
            Portrait = data.Portrait;
            QuestMarkerType = HubMapUIQuestMarkerType.None;

            QuestAnswers = new List<HubMapUIQuestAnswer>();
            for (int i = 0; i < data.QuestAnswers.Count; i++)
            {
                QuestAnswers.Add(new HubMapUIQuestAnswer(data.QuestAnswers[i]));
                QuestAnswers[i].Answer.OnAnswerSelectByPlayerHandler += SetCurrentDialogNode;
            }

            Dialogs = new List<HubMapUIDialogNode>();
            for (int i = 0; i < data.Dialogs.Count; i++)
            {
                Dialogs.Add(new HubMapUIDialogNode(data.Dialogs[i]));

                for (int j = 0; j < Dialogs[i].Answers.Count; j++)
                {
                    Dialogs[i].Answers[j].OnAnswerSelectByPlayerHandler += SetCurrentDialogNode;
                }
            }

            CurrentDialog = Dialogs.Find(dialog => dialog.Id == data.FirstDialogId);
        }

        #endregion


        #region Methods

        public void SetCurrentDialogNode(int nextDialogId)
        {
            HubMapUIDialogNode nextDialog = Dialogs.Find(dialog => dialog.Id == nextDialogId);
            if (nextDialog != null)
            {
                CurrentDialog = nextDialog;
            }
            else
            {
                Debug.LogError(this + "Dialog list does not contain requested ID. Citizen name: " + Name);
            }
        }

        public List<HubMapUIDialogAnswer> GetAllCurrentAnswers()
        {
            HubMapUIDialogAnswer additionalQuestAnswer = GetActiveQuestAnswerForCurrentDialog();
            if (additionalQuestAnswer == null)
            {
                return CurrentDialog.Answers;
            }
            else
            {
                List<HubMapUIDialogAnswer> list = new List<HubMapUIDialogAnswer>();
                list.Add(additionalQuestAnswer);
                for (int i = 0; i < CurrentDialog.Answers.Count; i++)
                {
                    list.Add(CurrentDialog.Answers[i]);
                }
                return list;
            }
        }

        public HubMapUIQuestAnswer GetQuestAnswerById(int id)
        {
            for (int i = 0; i < QuestAnswers.Count; i++)
            {
                if (QuestAnswers[i].Id == id)
                {
                    return QuestAnswers[i];
                }
            }
            return null;
        }

        private HubMapUIDialogAnswer GetActiveQuestAnswerForCurrentDialog()
        {
            for (int i = 0; i < QuestAnswers.Count; i++)
            {
                if (QuestAnswers[i].IsActivated && QuestAnswers[i].IsBelongToDialogNode(CurrentDialog.Id))
                {
                    QuestAnswers[i].SetInteractableThroughHandler();
                    return QuestAnswers[i].Answer;
                }
            }
            return null;
        }

        #endregion
    }
}
