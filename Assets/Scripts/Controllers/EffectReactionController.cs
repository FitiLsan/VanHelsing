using System;
using UnityEngine;

namespace BeastHunter
{
    public class EffectReactionController
    {
        #region Fields
        public Action<BuffEffectType> StartBuffEffect;
        public Action<BuffEffectType> EndBuffEffect;

        private CharacterModel _model; //base model?
        private EnemyModel _enemyModel;
        private GameContext _context;

        #endregion


        #region ClassLifeCycle

        public EffectReactionController(GameContext context, CharacterModel model)
        {
            _context = context;
            _model = model;
            
        }
        public EffectReactionController(GameContext context, EnemyModel model)
        {
            _context = context;
            _enemyModel = model;
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
            if (_enemyModel != null)
            {
                _enemyModel.CurrentStats.BuffHolder.BuffEffectEnable += StartReaction;
                _enemyModel.CurrentStats.BuffHolder.BuffEffectDisable += EndReaction;
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
            if (_enemyModel != null)
            {
                _enemyModel.CurrentStats.BuffHolder.BuffEffectEnable -= StartReaction;
                _enemyModel.CurrentStats.BuffHolder.BuffEffectDisable -= EndReaction;
            }
        }

        private void StartReaction(BuffEffectType type)
        {
            switch(type)
            {
                case BuffEffectType.Fire:
                    StartBuffEffect?.Invoke(type);
                    (_enemyModel as BossModel).BossStateMachine.SetCurrentStateAnyway(BossStatesEnum.Standstill, type);
                    break;
                case BuffEffectType.Blood:
                    break;
                case BuffEffectType.Poison:
                    break;
                case BuffEffectType.Slow:
                    break;
                case BuffEffectType.Water:
                    break;
            }
        }
        private void EndReaction(BuffEffectType type)
        {

        }

        #endregion
    }
}