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

        private void StartReaction(BuffEffectType type, BaseBuff buff)
        {
            if (TryCombineEffects(type, buff))
            {
                return;
            }


            switch (type)
            {
                case BuffEffectType.Burning:
                    Debug.Log("огонь");
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.SetCurrentStateAnyway(BossStatesEnum.Standstill, type);
                    }
                    break;
                case BuffEffectType.Wetting:
                    Debug.Log("вода");
                    break;
                case BuffEffectType.Poisoning:
                    break;
                case BuffEffectType.Freezing:
                    break;
                case BuffEffectType.Electrization:
                    break;
                case BuffEffectType.Bleeding:
                    break;
                case BuffEffectType.Slowing:
                    break;
                case BuffEffectType.Stunning:
                    break;
                case BuffEffectType.Overturning:
                    break;
                case BuffEffectType.Steaming:
                    Debug.Log("пар");
                    break;
                case BuffEffectType.Smoking:
                    break;

            }
        }
        private void EndReaction(BuffEffectType type)
        {
            switch (type)
            {
                case BuffEffectType.Burning:
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.CurrentState.OnExit();
                    }
                    break;
                case BuffEffectType.Wetting:
                    break;
                case BuffEffectType.Poisoning:
                    break;
                case BuffEffectType.Freezing:
                    break;
                case BuffEffectType.Electrization:
                    break;
                case BuffEffectType.Bleeding:
                    break;
                case BuffEffectType.Slowing:
                    break;
                case BuffEffectType.Stunning:
                    break;
                case BuffEffectType.Overturning:
                    break;
                case BuffEffectType.Steaming:
                    break;
                case BuffEffectType.Smoking:
                    break;
            }
        }


        private bool TryCombineEffects(BuffEffectType currentEffect, BaseBuff currentBuff)
        {
            if (_model == null)
            {
                return false;
            }
            foreach (var effect in Services.SharedInstance.EffectsManager.GetAllEffects())
            {
                TemporaryBuff buff;
                TemporaryBuff newBuff;
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