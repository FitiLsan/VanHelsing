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
                giantMudCrabBehaviour.OnTakeDamageHandler += OnTakeDamage;
                giantMudCrabBehaviour.Stats = _context.GiantMudCrabModel.GiantMudCrabStruct.Stats;
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
                giantMudCrabBehaviour.OnTakeDamageHandler -= OnTakeDamage;
            }
        }

        #endregion


        #region Methods

        private void OnTakeDamage(Damage damage)
        {
            if (_context.GiantMudCrabModel.GiantMudCrabStruct.IsDigIn)
            {
                _context.GiantMudCrabModel.CurrentHealth -= damage.PhysicalDamage / 2;
                Debug.Log("crab got " + damage.PhysicalDamage/2 + " damage");
            }
            else
            {
                _context.GiantMudCrabModel.CurrentHealth -= damage.PhysicalDamage;
                Debug.Log("crab got " + damage.PhysicalDamage + " damage");
            }

            float stunProbability = Random.Range(0f, 1f);

            if(damage.StunProbability > stunProbability)
            {
                Debug.Log("crab should be stunned");
            }

            if(_context.GiantMudCrabModel.CurrentHealth <= 0)
            {
                _context.GiantMudCrabModel.GiantMudCrabStruct.IsDead = true;
                Debug.Log("The crab is dead");
                _context.GiantMudCrabModel.Crab.GetComponent<Renderer>().material.color = Color.red;
                _context.GiantMudCrabModel.Crab.GetComponent<InteractableObjectBehavior>().enabled = false;
                Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.NpcDie, new EnemyDieArgs(_context.GiantMudCrabModel.GiantMudCrabTransform.GetComponent<IGetNpcInfo>().GetInfo().Item1,0));
            }
        }

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

