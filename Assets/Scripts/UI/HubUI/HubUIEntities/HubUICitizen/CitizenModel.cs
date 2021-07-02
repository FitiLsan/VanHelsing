using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class CitizenModel
    {
        #region Fields

        private QuestMarkerType _questMarkerType;

        #endregion


        #region Properties

        public Action<QuestMarkerType> OnChangeQuestMarkerTypeHandler { get; set; }

        public int InstanceId { get; private set; }
        public string Name { get; private set; }
        public Sprite Portrait { get; private set; }
        public List<QuestAnswer> QuestAnswers { get; set; }
        public List<DialogNode> Dialogs { get; private set; }
        public DialogNode CurrentDialog { get; private set; }

        public QuestMarkerType QuestMarkerType
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

        public CitizenModel(CitizenSO citizenSO)
        {
            InstanceId = citizenSO.InstanceId;
            Name = citizenSO.Name;
            Portrait = citizenSO.Portrait;
            QuestMarkerType = QuestMarkerType.None;

            QuestAnswers = new List<QuestAnswer>();
            for (int i = 0; i < citizenSO.QuestAnswers.Count; i++)
            {
                QuestAnswers.Add(new QuestAnswer(citizenSO.QuestAnswers[i]));
                QuestAnswers[i].Answer.OnAnswerSelectByPlayerHandler += SetCurrentDialogNode;
            }

            Dialogs = new List<DialogNode>();
            for (int i = 0; i < citizenSO.Dialogs.Count; i++)
            {
                Dialogs.Add(new DialogNode(citizenSO.Dialogs[i]));

                for (int j = 0; j < Dialogs[i].Answers.Count; j++)
                {
                    Dialogs[i].Answers[j].OnAnswerSelectByPlayerHandler += SetCurrentDialogNode;
                }
            }

            CurrentDialog = Dialogs.Find(dialog => dialog.Id == citizenSO.FirstDialogId);
        }

        #endregion


        #region Methods

        public void SetCurrentDialogNode(int nextDialogId)
        {
            DialogNode nextDialog = Dialogs.Find(dialog => dialog.Id == nextDialogId);
            if (nextDialog != null)
            {
                CurrentDialog = nextDialog;
            }
            else
            {
                Debug.LogError(this + "Dialog list does not contain requested ID. Citizen name: " + Name);
            }
        }

        public List<DialogAnswer> GetAllCurrentAnswers()
        {
            DialogAnswer additionalQuestAnswer = GetActiveQuestAnswerForCurrentDialog();
            if (additionalQuestAnswer == null)
            {
                return CurrentDialog.Answers;
            }
            else
            {
                List<DialogAnswer> list = new List<DialogAnswer>();
                list.Add(additionalQuestAnswer);
                for (int i = 0; i < CurrentDialog.Answers.Count; i++)
                {
                    list.Add(CurrentDialog.Answers[i]);
                }
                return list;
            }
        }

        public QuestAnswer GetQuestAnswerById(int id)
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

        private DialogAnswer GetActiveQuestAnswerForCurrentDialog()
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
