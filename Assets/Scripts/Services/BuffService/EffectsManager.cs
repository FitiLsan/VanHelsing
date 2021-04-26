using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class EffectsManager
    {
        private Dictionary<CombinedEffect, BuffEffectType> _combinedEffectDictionary = new Dictionary<CombinedEffect, BuffEffectType>();
        private List<BuffEffectType> _allEffectsTypes = new List<BuffEffectType>();

        public EffectsManager()
        {
            _combinedEffectDictionary.Add(new CombinedEffect(BuffEffectType.Burning, BuffEffectType.Wetting), BuffEffectType.Steaming);
            _combinedEffectDictionary.Add(new CombinedEffect(BuffEffectType.Wetting, BuffEffectType.Burning), BuffEffectType.Steaming);

            _allEffectsTypes.AddRange(new BuffEffectType[]
                    {   BuffEffectType.Burning,
                        BuffEffectType.Wetting,
                        BuffEffectType.Steaming
                    });
        }

        public TemporaryBuff GetEffectCombinationResult(BuffEffectType firstEffect, BuffEffectType secondEffect)
        {
            BuffEffectType effectType;
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

        public List<BuffEffectType> GetAllEffects()
        {
            return _allEffectsTypes;
        }
    }
}