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
        private EnemyModel _enemyModel;
        private IDisposable _noiseSubscribe;
        private IDisposable _smellSubscribe;


        #endregion


        #region ClassLifeCycle

        FeelingController(GameContext context, EnemyModel enemyModel)
        {
            _context = context;
            _enemyModel = enemyModel;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            _noiseSubscribe = MessageBroker.Default.Receive<Noise>().Subscribe(CatchNoise);
           // _smellSubscribe = MessageBroker.Default.Receive<Smell>().Subscribe(CatchSmell);
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
            var distance = (_enemyModel as BossModel).BossData.GetTargetDistance((_enemyModel as BossModel).BossCurrentPosition, noise.NoisePointPosition);
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

        //private void CatchSmell(Smell smell)
        //{
        //    var distance = (_enemyModel as BossModel).BossData.GetTargetDistance((_enemyModel as BossModel).BossCurrentPosition, smell.SmellPointPosition);
        //    if (smell.SmellingDistance < distance)
        //    {
        //        return;
        //    }

        //    switch (smell.Type)
        //    {
        //        case LureSmellTypeEnum.fungal:
        //            break;
        //        case LureSmellTypeEnum.meaty:
        //            break;
        //        case LureSmellTypeEnum.smoky:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion
    }
}