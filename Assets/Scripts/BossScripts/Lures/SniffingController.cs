using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace BeastHunter
{
    public class SniffingController
    {
        #region Fields

        private GameContext _context;
        private EnemyModel _enemyModel;
        private IDisposable _noiseSubscribe;
        

        #endregion


        #region ClassLifeCycle

        SniffingController(GameContext context, EnemyModel enemyModel)
        {
            _context = context;
            _enemyModel = enemyModel;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
          _noiseSubscribe= MessageBroker.Default.Receive<Noise>().Subscribe(CatchNoise);
        }
        public void Execute()
        {

        }
        public void OnTearDown()
        {
            _noiseSubscribe.Dispose();
        }

        private void CatchNoise(Noise noise)
        {

        }

        #endregion
    }
}