using DG.Tweening;
using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/UIIndication", order = 0)]
    public sealed class UIIndicationData : ScriptableObject
    {
        #region Fields

        public float DURATION = 0.75f;
        public float INTERVAL = 0.5f;

        public UIIndicationStruct UIIndicationStruct;
        [NonSerialized]
        public RectTransform CardIndicationUI;

        #endregion


        #region Methods

        public void GetCard(EventArgs args)
        {
            if (!(args is CardArgs cardArgs))
            {
                return;
            }
            Debug.Log($"Get new Card ID :{cardArgs.CardId}");
            StartIndication();
        }

        public void StartIndication()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(CardIndicationUI.DOAnchorPosX(350, DURATION))
                .AppendInterval(INTERVAL)
                .Append(CardIndicationUI.DOAnchorPosX(700, DURATION));

            #endregion
        }
    }
}