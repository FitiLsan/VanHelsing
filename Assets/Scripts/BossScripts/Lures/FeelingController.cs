using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace BeastHunter
{
    public class FeelingController
    {
        #region Fields

        private GameContext _context;
        private BaseModel _model;
        private IDisposable _noiseSubscribe;
        private IDisposable _smellSubscribe;


        #endregion


        #region ClassLifeCycle

        public FeelingController(GameContext context, BaseModel model)
        {
            _context = context;
            _model = model;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            //   _noiseSubscribe = MessageBroker.Default.Receive<Noise>().Subscribe(CatchNoise);
          //  _smellSubscribe = MessageBroker.Default.Receive<Smell>().Subscribe(CatchSmell);
           _noiseSubscribe = Services.SharedInstance.AnnouncementService.TakeNoise(CatchNoise);
            _smellSubscribe = Services.SharedInstance.AnnouncementService.TakeSmell(CatchSmell);
        }
        public void Execute()
        {

        }
        public void OnTearDown()
        {
            _noiseSubscribe.Dispose();
            _smellSubscribe.Dispose();
        }

        private void CatchNoise(Noise noise)
        {
            var distance = (_model as BossModel).BossData.GetTargetDistance((_model as BossModel).CurrentPosition, noise.NoisePointPosition);
            if (noise.HearingDistance < distance)
            {
                return;
            }

            switch(noise.Type)
            {
                case NoiseType.Explosion:
                    break;
                case NoiseType.FireBurning:
                    break;
                case NoiseType.GlassCrack:
                    break;
                case NoiseType.ObjectFall:
                    break;
                default:
                    break;
            }
        }

        private void CatchSmell(Smell smell)
        {
            var distance = _model.ThisEnemyData.GetTargetDistance(_model.CurrentPosition, smell.SmellPointPosition);
            if (smell.SmellingDistance < distance)
            {
                return;
            }
            //boss only
            if (_model is BossModel)
            {
                var model = (_model as BossModel);
                model.BossCurrentTarget = smell.SmellObject;
                CheckSmellAttitude(smell);
            }
        }

        private void CheckSmellAttitude(Smell smell)
        {
            if(_model.CurrentStats.ItemReactions.AttractiveItems.Find(x=> x.LureSmellType.Equals(smell.Type)))
            {
                (_model as BossModel).BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Moving);
            }
            else if(_model.CurrentStats.ItemReactions.ScaryItems.Find(x =>x.LureSmellType.Equals(smell.Type)))
            {
                (_model as BossModel).BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Retreating); // TODO logic in retreating state
            }
            else
            {
                (_model as BossModel).BossStateMachine.SetCurrentState(BossStatesEnum.Idle);
            }
        }

        #endregion
    }
}