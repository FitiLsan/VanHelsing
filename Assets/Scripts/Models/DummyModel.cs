using UnityEngine;
using DG.Tweening;
using TMPro;


namespace BeastHunter
{
    public sealed class DummyModel : EnemyModel, IAwake
    {

        #region Properties

        public Sequence DamageTextSequence { get; private set; }
        public Sequence DummyPushSequence { get; private set; }
        public Transform DamageTextObject { get; }
        public TextMeshPro DamageText { get; }
        public Color DamageTextColor { get; }
        public VisualEffectController VisualEffectController { get; }
        public EffectReactionController EffectReactionController { get; }

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
            BuffEffectPrefab = objectOnScene.transform.Find("Effects").gameObject;

            VisualEffectController = new VisualEffectController(Services.SharedInstance.Context, this);
            EffectReactionController = new EffectReactionController(Services.SharedInstance.Context, this);
        }

        #endregion


        #region Methods

        public override void TakeDamage(Damage damage)
        {
            Debug.Log($"Dummy recieved: PhysicalDamage:{damage.PhysicalDamageValue} Type: {damage.PhysicalDamageType} + ElementDamage:{damage.ElementDamageValue} Type: {damage.ElementDamageType} and has { CurrentStats.BaseStats.CurrentHealthPoints} of HP");

            ((DummyData)ThisEnemyData).CreateDamageObject(damage, this);
            if (!damage.IsEffectDamage)
            {
                var elementEffect = Services.SharedInstance.EffectsManager.GetEffectByElementDamageType(damage.ElementDamageType);
                var physicEffect = Services.SharedInstance.EffectsManager.GetEffectByPhysicalDamageType(damage.PhysicalDamageType);
                
                Services.SharedInstance.BuffService.AddTemporaryBuff(InstanceID, Resources.Load($"Data/Buffs/BaseDebuffs/{physicEffect}") as TemporaryBuff);
                Services.SharedInstance.BuffService.AddTemporaryBuff(InstanceID, Resources.Load($"Data/Buffs/BaseDebuffs/{elementEffect}") as TemporaryBuff);
            }
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

        public void OnAwake()
        {
            VisualEffectController.OnAwake();
            EffectReactionController.OnAwake();
        }


        #endregion
    }
}

