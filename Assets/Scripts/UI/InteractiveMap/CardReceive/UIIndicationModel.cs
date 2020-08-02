using UnityEngine;


namespace BeastHunter
{
    public sealed class UIIndicationModel
    {
        #region Fields

        public GameContext Context;

        #endregion


        #region Properties

        public UIIndicationData UIIndicationData { get; }
        public UIIndicationStruct UIIndicationStruct { get; }

        #endregion


        #region ClassLifeCycle

        public UIIndicationModel(GameObject model, UIIndicationData uiIndicationData, GameContext context)
        {
            Context = context;
            UIIndicationData = uiIndicationData;
            UIIndicationData.CardIndicationUI = model.GetComponent<RectTransform>();
            Services.SharedInstance.EventManager.StartListening(GameEventTypes.ReceivingNewCard, UIIndicationData.GetCard);
           //QuestModel.ReceiveCardEvent += UIIndicationData.GetCard;
        }

        #endregion


        #region Metods

        public void Execute()
        {
            if (Input.GetButtonDown("Jump"))
            {
               UIIndicationData.StartIndication();
            }
        }

        #endregion
    }
}