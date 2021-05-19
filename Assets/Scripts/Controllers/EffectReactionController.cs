using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeastHunter
{

    public class EffectReactionController
    {
        #region Fields

        private BaseModel _model;
        private GameContext _context;

        #endregion


        #region ClassLifeCycle

        public EffectReactionController(GameContext context, BaseModel model)
        {
            _context = context;
            _model = model;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            if (_model != null)
            {
                _model.CurrentStats.BuffHolder.BuffEffectEnable += StartReaction;
                _model.CurrentStats.BuffHolder.BuffEffectDisable += EndReaction;
            }
        }
        public void Execute()
        {

        }
        public void OnTearDown()
        {
            if (_model != null)
            {
                _model.CurrentStats.BuffHolder.BuffEffectEnable -= StartReaction;
                _model.CurrentStats.BuffHolder.BuffEffectDisable -= EndReaction;
            }
        }

        private void StartReaction(EffectType type, BaseBuff buff)
        {
            if (TryCombineEffects(type, buff))
            {
                return;
            }


            switch (type)
            {
                case EffectType.Burning:
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.SetCurrentStateAnyway(BossStatesEnum.Standstill, type);
                    }
                    break;
                case EffectType.Wetting:
                    break;
                case EffectType.Freezing:
                    break;
                case EffectType.Electrization:
                    break;
                case EffectType.Oiling:
                    break;
                case EffectType.Poisoning:
                    break;
                case EffectType.Gassing:
                    break;
                case EffectType.Suffocation:
                    break;
                case EffectType.Bleeding:
                    break;
                case EffectType.Stunning:
                    break;
                case EffectType.Slowing:
                    break;
                case EffectType.Overturning:
                    break;
                case EffectType.Contusion:
                    break;
                case EffectType.Intoxication:
                    break;
                case EffectType.Blinding:
                    break;
                case EffectType.Explosion:
                    break;
                default:
                    CustomDebug.LogError($"Type {type} does not exist");
                    break;

            }
        }
        private void EndReaction(EffectType type)
        {
            switch (type)
            {
                case EffectType.Burning:
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.CurrentState.OnExit();
                    }
                    break;
                case EffectType.Wetting:
                    break;
                case EffectType.Freezing:
                    break;
                case EffectType.Electrization:
                    break;
                case EffectType.Oiling:
                    break;
                case EffectType.Poisoning:
                    break;
                case EffectType.Gassing:
                    break;
                case EffectType.Suffocation:
                    break;
                case EffectType.Bleeding:
                    break;
                case EffectType.Stunning:
                    break;
                case EffectType.Slowing:
                    break;
                case EffectType.Overturning:
                    break;
                case EffectType.Contusion:
                    break;
                case EffectType.Intoxication:
                    break;
                case EffectType.Blinding:
                    break;
                case EffectType.Explosion:
                    break;
                default:
                    CustomDebug.LogError($"Type {type} does not exist");
                    break;
            }
        }


        private bool TryCombineEffects(EffectType currentEffect, BaseBuff currentBuff)
        {
            if (_model == null)
            {
                return false;
            }

            var allBuffEffects  = _model.CurrentStats.BuffHolder.TemporaryBuffList.SelectMany(x => x.Effects);//Services.SharedInstance.EffectsManager.GetAllEffects();

            foreach (var buffEffect in allBuffEffects)
            {
                TemporaryBuff buff;
                TemporaryBuff newBuff;
                var effect = buffEffect.BuffEffectType;

                if (currentEffect != effect && (buff = _model.CurrentStats.BuffHolder.TemporaryBuffList.Find(x => x.Effects.Any(y => y.BuffEffectType.Equals(effect)))))
                {
                    newBuff = Services.SharedInstance.EffectsManager.GetEffectCombinationResult(effect, currentEffect);
                    if (newBuff != null)
                    {
                        Services.SharedInstance.BuffService.RemoveTemporaryBuff(_model.CurrentStats, buff, _model.CurrentStats.BuffHolder);
                        Services.SharedInstance.BuffService.RemoveTemporaryBuff(_model.CurrentStats, (currentBuff as TemporaryBuff), _model.CurrentStats.BuffHolder);
                        Services.SharedInstance.BuffService.AddTemporaryBuff(_model.CurrentStats.InstanceID, newBuff);
                        return true;
                    }
                }
            }
            return false;
        }
        
        #endregion
    }
}