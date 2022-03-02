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
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Freezing), EffectType.Wetting);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Oiling), EffectType.Burning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Gassing), EffectType.Explosion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Bleeding), EffectType.Burning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Burning, EffectType.Contusion), EffectType.Burning);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Wetting, EffectType.Burning), EffectType.Suffocation);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Wetting, EffectType.Freezing), EffectType.Freezing);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Wetting, EffectType.Oiling), EffectType.Oiling);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Wetting, EffectType.Intoxication), EffectType.Wetting);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Freezing, EffectType.Burning), EffectType.Wetting);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Freezing, EffectType.Wetting), EffectType.Freezing);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Freezing, EffectType.Gassing), EffectType.Poisoning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Freezing, EffectType.Bleeding), EffectType.Freezing);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Freezing, EffectType.Explosion), EffectType.Explosion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Electrization, EffectType.Gassing), EffectType.Explosion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Electrization, EffectType.Bleeding), EffectType.Electrization);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Electrization, EffectType.Intoxication), EffectType.Electrization);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Oiling, EffectType.Burning), EffectType.Burning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Oiling, EffectType.Wetting), EffectType.Oiling);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Poisoning, EffectType.Burning), EffectType.Gassing);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Poisoning, EffectType.Intoxication), EffectType.Blinding);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Gassing, EffectType.Burning), EffectType.Explosion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Gassing, EffectType.Electrization), EffectType.Explosion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Gassing, EffectType.Explosion), EffectType.Explosion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Suffocation, EffectType.Explosion), EffectType.Explosion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Bleeding, EffectType.Burning), EffectType.Burning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Bleeding, EffectType.Freezing), EffectType.Freezing);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Stunning, EffectType.Overturning), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Stunning, EffectType.Contusion), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Stunning, EffectType.Intoxication), EffectType.Overturning);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Overturning, EffectType.Stunning), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Overturning, EffectType.Contusion), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Overturning, EffectType.Intoxication), EffectType.Contusion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Contusion, EffectType.Burning), EffectType.Burning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Contusion, EffectType.Wetting), EffectType.Wetting);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Contusion, EffectType.Electrization), EffectType.Electrization);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Contusion, EffectType.Stunning), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Contusion, EffectType.Overturning), EffectType.Contusion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Wetting), EffectType.Wetting);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Electrization), EffectType.Electrization);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Poisoning), EffectType.Blinding);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Stunning), EffectType.Overturning);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Overturning), EffectType.Contusion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Intoxication, EffectType.Contusion), EffectType.Contusion);

            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Explosion, EffectType.Gassing), EffectType.Explosion);
            _combinedEffectDictionary.Add(new CombinedEffect(EffectType.Explosion, EffectType.Suffocation), EffectType.Explosion);

            _allEffectsTypes.AddRange(new EffectType[]
                    {    EffectType.Wetting,
                         EffectType.Freezing,
                         EffectType.Electrization,
                         EffectType.Oiling,
                         EffectType.Poisoning,
                         EffectType.Gassing,
                         EffectType.Suffocation,
                         EffectType.Bleeding,
                         EffectType.Stunning,
                         EffectType.Slowing,
                         EffectType.Overturning,
                         EffectType.Contusion,
                         EffectType.Intoxication,
                         EffectType.Blinding,
                         EffectType.Explosion,
                    });
        }

        public List<EffectType> GetAllEffects()
        {
            return _allEffectsTypes;
        }

        public TemporaryBuff GetEffectCombinationResult(EffectType firstEffect, EffectType secondEffect)
        {
            EffectType effectType;
            if (!_combinedEffectDictionary.TryGetValue(new CombinedEffect(firstEffect, secondEffect), out effectType))
            {
                return null;
            }
            var newEffect = new BuffEffect();
            newEffect.Buff = Buff.None;
            newEffect.BuffEffectType = effectType;
            newEffect.IsTicking = false;
            newEffect.Value = 0;

            var newBuff = new TemporaryBuff();
            newBuff.Name = newEffect.BuffEffectType.ToString();
            newBuff.Effects = new BuffEffect[1];
            newBuff.Effects[0] = newEffect;
            newBuff.Time = 15;
            newBuff.Type = BuffType.Debuf;

            return newBuff;
        }

        public ElementDamageType GetElementByEffect(EffectType effectType)
        {
            ElementDamageType elementType;
            switch (effectType)
            {
                case EffectType.Burning:
                    elementType = ElementDamageType.Fire;
                    break;
                case EffectType.Wetting:
                    elementType = ElementDamageType.Water;
                    break;
                case EffectType.Freezing:
                    elementType = ElementDamageType.Ice;
                    break;
                case EffectType.Electrization:
                    elementType = ElementDamageType.Electricity;
                    break;
                case EffectType.Oiling:
                    elementType = ElementDamageType.Oil;
                    break;
                case EffectType.Poisoning:
                    elementType = ElementDamageType.Toxin;
                    break;
                case EffectType.Gassing:
                    elementType = ElementDamageType.Gas;
                    break;
                case EffectType.Suffocation:
                    elementType = ElementDamageType.SmokeAndSteam;
                    break;
                case EffectType.Bleeding:
                    elementType = ElementDamageType.Bleeding;
                    break;

                default:
                    CustomDebug.LogError($"Type {effectType} does not exist");
                    elementType = ElementDamageType.None;
                    break;
            }
            return elementType;
        }

        public float GetEffectProbability(EffectType effectType, Stats stats)
        {
            float effectProbability;
            switch (effectType)
            {
                case EffectType.Burning:
                    effectProbability = stats.DefenceStats.BurningProbabilityResistance;
                    break;
                case EffectType.Wetting:
                    effectProbability = stats.DefenceStats.WeetingProbabilityResistance;
                    break;
                case EffectType.Freezing:
                    effectProbability = stats.DefenceStats.FreezingProbabilityResistance;
                    break;
                case EffectType.Electrization:
                    effectProbability = stats.DefenceStats.ElectrizationProbabilityResistance;
                    break;
                case EffectType.Oiling:
                    effectProbability = stats.DefenceStats.OilingProbabilityResistance;
                    break;
                case EffectType.Poisoning:
                    effectProbability = stats.DefenceStats.PoisoningProbabilityResistance;
                    break;
                case EffectType.Gassing:
                    effectProbability = stats.DefenceStats.GassingProbabilityResistance;
                    break;
                case EffectType.Suffocation:
                    effectProbability = stats.DefenceStats.SuffocationProbabilityResistance;
                    break;
                case EffectType.Bleeding:
                    effectProbability = stats.DefenceStats.BleedingProbabilityResistance;
                    break;
                case EffectType.Stunning:
                    effectProbability = stats.DefenceStats.StunningProbabilityResistance;
                    break;
                case EffectType.Slowing:
                    effectProbability = stats.DefenceStats.SlowingProbabilityResistance;
                    break;
                case EffectType.Overturning:
                    effectProbability = stats.DefenceStats.OverturningProbabilityResistance;
                    break;
                case EffectType.Contusion:
                    effectProbability = stats.DefenceStats.ContusionProbabilityResistance;
                    break;
                case EffectType.Intoxication:
                    effectProbability = stats.DefenceStats.IntoxicationProbabilityResistance;
                    break;
                case EffectType.Blinding:
                    effectProbability = stats.DefenceStats.BlindingProbabilityResistance;
                    break;
                case EffectType.Explosion:
                    effectProbability = stats.DefenceStats.ExplosionProbabilityResistance;
                    break;
                default:
                    CustomDebug.LogError($"Type {effectType} does not exist");
                    effectProbability = 0;
                    break;
            }
            return effectProbability;
        }

        public EffectType GetEffectByElementDamageType(ElementDamageType type)
        {
            EffectType effectType; ;
            switch (type)
            {
                case ElementDamageType.Fire:
                    {
                        effectType = EffectType.Burning;
                    }
                    break;
                case ElementDamageType.Water:
                    {
                        effectType = EffectType.Wetting;
                    }
                    break;
                case ElementDamageType.Ice:
                    {
                        effectType = EffectType.Freezing;
                    }
                    break;
                case ElementDamageType.Electricity:
                    {
                        effectType = EffectType.Electrization;
                    }
                    break;
                case ElementDamageType.Oil:
                    {
                        effectType = EffectType.Oiling;
                    }
                    break;
                case ElementDamageType.Toxin:
                    {
                        effectType = EffectType.Poisoning;
                    }
                    break;
                case ElementDamageType.Gas:
                    {
                        effectType = EffectType.Gassing;
                    }
                    break;
                case ElementDamageType.SmokeAndSteam:
                    {
                        effectType = EffectType.Suffocation;
                    }
                    break;
                default:
                    effectType = EffectType.None;
                    break;
            }
            return effectType;
        }

        public EffectType GetEffectByPhysicalDamageType(PhysicalDamageType type)
        {
            EffectType effectType;
            switch (type)
            {
                case PhysicalDamageType.Cutting:
                    {
                        effectType = EffectType.Bleeding;
                    }
                    break;
                case PhysicalDamageType.Piercing:
                    {
                        effectType = EffectType.Bleeding;
                    }
                    break;
                case PhysicalDamageType.Chopping:
                    {
                        effectType = Random.Range(1, 3) == 1 ? EffectType.Stunning : EffectType.Overturning;
                    }
                    break;
                case PhysicalDamageType.Crushing:
                    {
                        effectType = Random.Range(1, 3) == 1 ? EffectType.Stunning : EffectType.Overturning;
                    }
                    break;
                case PhysicalDamageType.Penetration:
                    {
                        effectType = EffectType.Bleeding;
                    }
                    break;
                case PhysicalDamageType.Explosion:
                    {
                        effectType = Random.Range(1, 3) == 1 ? EffectType.Contusion : EffectType.Overturning;
                    }
                    break;
                default:
                    effectType = EffectType.None;
                    break;
            }
            return effectType;
        }
        public float GetElementResistance(ElementDamageType type, Stats receiverStats)
        {
            float resistanceValue;
            switch (type)
            {
                case ElementDamageType.Fire:
                    {
                        resistanceValue = receiverStats.DefenceStats.FireDamageResistance;
                    }
                    break;
                case ElementDamageType.Water:
                    {
                        resistanceValue = receiverStats.DefenceStats.WaterDamageResistance;
                    }
                    break;
                case ElementDamageType.Ice:
                    {
                        resistanceValue = receiverStats.DefenceStats.IceDamageResistance;
                    }
                    break;
                case ElementDamageType.Electricity:
                    {
                        resistanceValue = receiverStats.DefenceStats.ElectricityDamageResistance;
                    }
                    break;
                case ElementDamageType.Oil:
                    {
                        resistanceValue = receiverStats.DefenceStats.OilDamageResistance;
                    }
                    break;
                case ElementDamageType.Toxin:
                    {
                        resistanceValue = receiverStats.DefenceStats.ToxinDamageResistance;
                    }
                    break;
                case ElementDamageType.Gas:
                    {
                        resistanceValue = receiverStats.DefenceStats.GasDamageResistance;
                    }
                    break;
                case ElementDamageType.SmokeAndSteam:
                    {
                        resistanceValue = receiverStats.DefenceStats.SmokeAndSteamDamageResistance;
                    }
                    break;
                default:
                    resistanceValue = 0;
                    break;
            }
            return resistanceValue;
        }

        public float GetPhysicalResistance(PhysicalDamageType type, Stats receiverStats)
        {
            float resistanceValue;
            switch (type)
            {
                case PhysicalDamageType.Cutting:
                    {
                        resistanceValue = receiverStats.DefenceStats.CuttingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Piercing:
                    {
                        resistanceValue = receiverStats.DefenceStats.PiercingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Chopping:
                    {
                        resistanceValue = receiverStats.DefenceStats.ChoppingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Crushing:
                    {
                        resistanceValue = receiverStats.DefenceStats.CrushingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Penetration:
                    {
                        resistanceValue = receiverStats.DefenceStats.PenetrationDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Explosion:
                    {
                        resistanceValue = receiverStats.DefenceStats.ExplosionProbabilityResistance;
                    }
                    break;
                default:
                    resistanceValue = 0f;
                    break;
            }
            return resistanceValue;
        }

        public float GetPhysicalPower(PhysicalDamageType type, Stats dealerStats, bool isNeedCheckPower)
        {
            if (!isNeedCheckPower)
            {
                return 0;
            }
            float powerValue;
            switch (type)
            {
                case PhysicalDamageType.Cutting:
                    {
                        powerValue = dealerStats.AttackStats.CuttingPower;
                    }
                    break;
                case PhysicalDamageType.Piercing:
                    {
                        powerValue = dealerStats.AttackStats.PiersingPower;
                    }
                    break;
                case PhysicalDamageType.Chopping:
                    {
                        powerValue = dealerStats.AttackStats.ChoppingPower;
                    }
                    break;
                case PhysicalDamageType.Crushing:
                    {
                        powerValue = dealerStats.AttackStats.CrushingPower;
                    }
                    break;
                case PhysicalDamageType.Penetration:
                    {
                        powerValue = dealerStats.AttackStats.PenetrationPower;
                    }
                    break;
                case PhysicalDamageType.Explosion:
                    {
                        powerValue = dealerStats.AttackStats.ExplosionPower;
                    }
                    break;
                default:
                    powerValue = 0f;
                    break;
            }
            return powerValue;
        }

        public float GetElementPower(ElementDamageType type, Stats receiverStats, bool isNeedCheckPower)
        {
            if (!isNeedCheckPower)
            {
                return 0;
            }
            float powerValue;
            switch (type)
            {
                case ElementDamageType.Fire:
                    {
                        powerValue = receiverStats.AttackStats.FirePower;
                    }
                    break;
                case ElementDamageType.Water:
                    {
                        powerValue = receiverStats.AttackStats.WaterPower;
                    }
                    break;
                case ElementDamageType.Ice:
                    {
                        powerValue = receiverStats.AttackStats.IcePower;
                    }
                    break;
                case ElementDamageType.Electricity:
                    {
                        powerValue = receiverStats.AttackStats.ElectricityPower;
                    }
                    break;
                case ElementDamageType.Oil:
                    {
                        powerValue = receiverStats.AttackStats.OilPower;
                    }
                    break;
                case ElementDamageType.Toxin:
                    {
                        powerValue = receiverStats.AttackStats.ToxinPower;
                    }
                    break;
                case ElementDamageType.Gas:
                    {
                        powerValue = receiverStats.AttackStats.GasPower;
                    }
                    break;
                case ElementDamageType.SmokeAndSteam:
                    {
                        powerValue = receiverStats.AttackStats.SmokeAndSteamPower;
                    }
                    break;
                default:
                    powerValue = 0;
                    break;
            }
            return powerValue;
        }
    }
}