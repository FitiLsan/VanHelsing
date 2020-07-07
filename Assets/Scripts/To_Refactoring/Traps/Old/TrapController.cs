using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class TrapController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public TrapController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            if(_context.TrapModel != null)
            {
                _context.TrapModel.Execute();
                this.OnAwake();
            }
            else
            {
                //Debug.Log("Ray not found Ground");
            }
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            //var Traps = _context.GetTriggers(InteractableObjectType.Trap);
            //foreach (var trigger in Traps)
            //{
            //    var trapBehaviour = trigger as TrapBehaviour;
            //    trapBehaviour.OnFilterHandler += OnFilterHandler;
            //    trapBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
            //    trapBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            //    //trapBehaviour.OnTakeDamageHandler += OnTakeDamage;
            //    //trapBehaviour.Stats = _context.GiantMudCrabModel.GiantMudCrabStruct.Stats;
            //    Debug.Log("Activate");
            //}
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            //var Traps = _context.GetTriggers(InteractableObjectType.Trap);
            //foreach (var trigger in Traps)
            //{
            //    var trapBehaviour = trigger as TrapBehaviour;
            //    trapBehaviour.OnFilterHandler -= OnFilterHandler;
            //    trapBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
            //    trapBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            //    //trapBehaviour.OnTakeDamageHandler -= OnTakeDamage;
            //}
        }

        #endregion


        #region Metods

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject, Collider other)
        {
            enteredObject.IsInteractable = true;
            _context.GiantMudCrabModel.GiantMudCrabStruct.CanAttack = true;
            _context.GiantMudCrabModel.GiantMudCrabStruct.IsPatrol = false;
            Debug.Log("Enter");
        }

        private void OnTriggerExitHandler(ITrigger enteredObject, Collider other)
        {
            enteredObject.IsInteractable = false;
            _context.GiantMudCrabModel.GiantMudCrabStruct.CanAttack = false;
            _context.GiantMudCrabModel.GiantMudCrabStruct.IsPatrol = true;
            Debug.Log("Exit");
        }


        #endregion
    }

}
