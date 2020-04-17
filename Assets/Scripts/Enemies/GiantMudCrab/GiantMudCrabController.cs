using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public class GiantMudCrabController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public GiantMudCrabController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            _context.GiantMudCrabModel.Execute();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _context.GiantMudCrabModel.CrabAgent = _context.GiantMudCrabModel.Crab.GetComponent<NavMeshAgent>();
            _context.GiantMudCrabModel.GiantMudCrabData.NextAttackRate = 0;

            var Crabs = _context.GetTriggers(InteractableObjectType.Crab);
            foreach (var trigger in Crabs)
            {
                var giantMudCrabBehaviour = trigger as GiantMudCrabBehaviour;
                giantMudCrabBehaviour.OnFilterHandler += OnFilterHandler;
                giantMudCrabBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                giantMudCrabBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
                Debug.Log("Activate");
            }
        }

        #endregion

        #region ITearDownController

        public void TearDown()
        {
            var Crabs = _context.GetTriggers(InteractableObjectType.Crab);
            foreach (var trigger in Crabs)
            {
                var giantMudCrabBehaviour = trigger as GiantMudCrabBehaviour;
                giantMudCrabBehaviour.OnFilterHandler -= OnFilterHandler;
                giantMudCrabBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                giantMudCrabBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region Methods

        private bool OnFilterHandler(Collider tagObject)
        {
            return tagObject.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
            _context.GiantMudCrabModel.GiantMudCrabStruct.CanAttack = true;
            _context.GiantMudCrabModel.GiantMudCrabStruct.IsPatrol = false;
            Debug.Log("Enter");
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            _context.GiantMudCrabModel.GiantMudCrabStruct.CanAttack = false;
            _context.GiantMudCrabModel.GiantMudCrabStruct.IsPatrol = true;
            Debug.Log("Exit");
        }

        #endregion
    }
}

