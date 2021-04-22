using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeastHunter
{
    public struct CombineEffect
    {
        public BuffEffectType firstEffect;
        public BuffEffectType secondEffect;
        
        public CombineEffect(BuffEffectType firstEffect, BuffEffectType secondEffect)
        {
            this.firstEffect = firstEffect;
            this.secondEffect = secondEffect;
        }
    }
    public class EffectReactionController
    {
        #region Fields

        private BaseModel _model;
        private GameContext _context;

        private Dictionary<CombineEffect, BuffEffectType> CombineEffectDic = new Dictionary<CombineEffect, BuffEffectType>();

        #endregion


        #region ClassLifeCycle

        public EffectReactionController(GameContext context, BaseModel model)
        {
            _context = context;
            _model = model;
            CombineEffectDic.Add(new CombineEffect(BuffEffectType.Fire, BuffEffectType.Water), BuffEffectType.Steam);
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
            switch (type)
            {
                case BuffEffectType.Fire:
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.SetCurrentStateAnyway(BossStatesEnum.Standstill, type);
                    }
                    break;
                case BuffEffectType.Water:
                    WaterReaction(buff);
                    break;
                case BuffEffectType.Poison:
                    break;
                case BuffEffectType.Frosting:
                    break;
                case BuffEffectType.Electricity:
                    break;
                case BuffEffectType.Bleeding:
                    break;
                case BuffEffectType.Slowing:
                    break;
                case BuffEffectType.Stunning:
                    break;
                case BuffEffectType.Overturning:
                    break;
                case BuffEffectType.Steam:
                    break;
                case BuffEffectType.Smoke:
                    break;

            }
        }
        private void EndReaction(BuffEffectType type)
        {
            switch (type)
            {
                case BuffEffectType.Fire:
                    if (_model != null && _model is BossModel)
                    {
                        (_model as BossModel).BossStateMachine.CurrentState.OnExit();
                    }
                    break;
                case BuffEffectType.Water:
                    break;
                case BuffEffectType.Poison:
                    break;
                case BuffEffectType.Frosting:
                    break;
                case BuffEffectType.Electricity:
                    break;
                case BuffEffectType.Bleeding:
                    break;
                case BuffEffectType.Slowing:
                    break;
                case BuffEffectType.Stunning:
                    break;
                case BuffEffectType.Overturning:
                    break;
                case BuffEffectType.Steam:
                    break;
                case BuffEffectType.Smoke:
                    break;
            }
        }


        private void WaterReaction(BaseBuff currentBuff)
        {
            if (_model != null)
            {
                TemporaryBuff buff;
                if (buff = _model.CurrentStats.BuffHolder.TemporaryBuffList.Find(x => x.Effects.Any(y => y.BuffEffectType.Equals(BuffEffectType.Fire))))
                {
                    _model.CurrentStats.BuffHolder.RemoveTemporaryBuff(buff);
                    _model.CurrentStats.BuffHolder.RemoveTemporaryBuff(currentBuff as TemporaryBuff);
                    _model.CurrentStats.BuffHolder.AddTemporaryBuff(GetEffectCombinationResult(BuffEffectType.Fire, BuffEffectType.Water));
                }
            }

            #endregion
        }
        
        private TemporaryBuff GetEffectCombinationResult(BuffEffectType firstEffect, BuffEffectType secondEffect)
        {
            var effectType = CombineEffectDic[new CombineEffect(firstEffect, secondEffect)];
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
    }
}