using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class ButterflyController: IAwake, IUpdate
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public ButterflyController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.ButterflyModel.Act();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var butterflies = _context.GetTriggers(InteractableObjectType.Butterfly);
            foreach (var trigger in butterflies)
            {
                ButterflyBehaviour butterflyBehaviour = trigger as ButterflyBehaviour;
                butterflyBehaviour.OnTriggerEnterHandler += OnTriggerEnter;
            }
        }

        private void OnTriggerEnter(ITrigger trigger, Collider collider)
        {
            Debug.Log("Butterfly OnTriggerEnter");
            //_context.ButterflyModel.TargetPoint = 
        }

        #endregion
    }
}
