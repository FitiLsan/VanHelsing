using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class RabbitController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public RabbitController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.RabbitModel.Execute();
            //foreach (var rabbit in _context.RabbitModels)
            //{
            //    rabbit.Execute();
            //}
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var Rabbits = _context.GetTriggers(InteractableObjectType.Rabbit);
            foreach (var trigger in Rabbits)
            {
                var rabbitBehaviour = trigger as RabbitBehaviour;
                rabbitBehaviour.OnTakeDamageHandler += OnTakeDamage;
                Debug.Log("ActivateRabbit");
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var Rabbits = _context.GetTriggers(InteractableObjectType.Rabbit);
            foreach (var trigger in Rabbits)
            {
                var rabbitBehaviour = trigger as RabbitBehaviour;
                rabbitBehaviour.OnTakeDamageHandler -= OnTakeDamage;
            }
        }

        #endregion


        #region Methods

        private void OnTakeDamage(Damage damage)
        {
            //_context.RabbitModels.CurrentHealth -= damage.damage;
            _context.RabbitModel.CurrentHealth -= damage.PhysicalDamage;
            Debug.Log("Rabbit got " + damage.PhysicalDamage + " damage");

            if (_context.RabbitModel.CurrentHealth <= 0)
            {
                _context.RabbitModel.IsDead = true;
                Debug.Log("You killed a bunny! You monster!");
                _context.RabbitModel.Rabbit.GetComponent<Renderer>().material.color = Color.red;
                _context.RabbitModel.Rabbit.GetComponent<InteractableObjectBehavior>().enabled = false;
            }
        }

        #endregion
    }
}
