using UnityEngine;
using DG.Tweening;
using TMPro;


namespace BeastHunter
{
    public sealed class DummyModel : EnemyModel
    {

        #region Properties

        public Sequence DamageTextSequence { get; private set; }
        public Sequence DummyPushSequence { get; private set; }
        public Transform DamageTextObject { get; }
        public TextMeshPro DamageText { get; }
        public Color DamageTextColor { get; }

        #endregion


        #region ClassLifeCycle

        public DummyModel(GameObject objectOnScene, EnemyData data) : base(objectOnScene, data)
        {
            DamageTextSequence = DOTween.Sequence();
            DummyPushSequence = DOTween.Sequence();
            DamageTextObject = GameObject.Instantiate(((DummyData)ThisEnemyData).DamageTextObject,
                ObjectOnScene.transform.position + Vector3.up * ((DummyData)ThisEnemyData).DummyHeight,
                    Quaternion.identity).transform;
            DamageText = DamageTextObject.GetComponent<TextMeshPro>();
            DamageTextColor = DamageText.color;
            DamageText.text = string.Empty;
            DamageText.color = Color.clear;
        }

        #endregion


        #region Methods

        public override void TakeDamage(Damage damage)
        {
            ((DummyData)ThisEnemyData).CreateDamageObject(damage, this);
        }

        public void RefreshDamageTextSequence(bool doComplete)
        {
            if (doComplete && !DamageTextSequence.IsComplete()) DamageTextSequence.Complete();
            DamageTextSequence = DOTween.Sequence();
        }

        public void RefreshDummyPushSeqnece(bool doComplete)
        {
            if (doComplete && !DummyPushSequence.IsComplete()) DummyPushSequence.Complete();
            DummyPushSequence = DOTween.Sequence();
        }

        #endregion
    }
}

