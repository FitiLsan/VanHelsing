using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class EffectsManager
    {
        private Dictionary<CombinedEffect, EffectType> _combinedEffectDictionary = new Dictionary<CombinedEffect, EffectType>();
        private List<EffectType> _allEffectsTypes = new List<EffectType>();

        public EffectsManager()
        {
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Wetting), EffectType.Suffocation);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Wetting, EffectType.Burning), EffectType.Suffocation);

            _allEffectsTypes.AddRange(new EffectType[]
                    {   EffectType.Burning,
                        EffectType.Wetting,
                        EffectType.Suffocation
                    });
        }

        public TemporaryBuff GetEffectCombinationResult(EffectType firstEffect, EffectType secondEffect)
        {
            EffectType effectType;
            if (!_combinedEffectDictionary.TryGetValue(new CombinedEffect(firstEffect, secondEffect), out effectType))
            {
                return null;
            }
            var newEffect = new BuffEffect();
            newEffect.Buff = Buff.SpeedChangeValue;
            newEffect.BuffEffectType = effectType;
            newEffect.IsTicking = false;
            newEffect.Value = 2f;

            var newBuff = new TemporaryBuff();
            newBuff.Effects = new BuffEffect[1];
            newBuff.Effects[0] = newEffect;
            newBuff.Time = 15;
            newBuff.Type = BuffType.Debuf;

            return newBuff;
        }

        public List<EffectType> GetAllEffects()
        {
            return _allEffectsTypes;
        }
    }
}