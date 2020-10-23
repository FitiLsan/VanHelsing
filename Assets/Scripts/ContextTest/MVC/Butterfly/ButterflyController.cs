using UnityEngine;


namespace BeastHunter
{
    public sealed class ButterflyController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private GameContext _context;

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
            Debug.Log("update");
            _context.ButterflyModel.Execute();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var Butterfly = _context.GetTriggers(InteractableObjectType.Butterfly);
            foreach (var trigger in Butterfly)
            {
                var butterflyBehaviour = trigger as ButterflyBehaviour;
                butterflyBehaviour.OnTakeDamageHandler += OnTakeDamage;
                butterflyBehaviour.Stats = _context.ButterflyModel.ButterflyData.ButterflyStruct.Stats;
                Debug.Log("ActivateButterfly");
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var Butterfly = _context.GetTriggers(InteractableObjectType.Butterfly);
            foreach (var trigger in Butterfly)
            {
                var butterflyBehaviour = trigger as ButterflyBehaviour;
                butterflyBehaviour.OnTakeDamageHandler -= OnTakeDamage;
            }
        }

        #endregion


        #region Methods

        private void OnTakeDamage(Damage damage)
        {
            _context.ButterflyModel.CurrentHealth -= damage.PhysicalDamage;
            Debug.Log("Butterfly got " + damage.PhysicalDamage + " damage");

            if (_context.ButterflyModel.CurrentHealth <= 0)
            {
                _context.ButterflyModel.IsDead = true;
                Debug.Log("You killed a Butterfly! You monster!");
                _context.ButterflyModel.Butterfly.GetComponent<Renderer>().material.color = Color.red;
                _context.ButterflyModel.Butterfly.GetComponent<InteractableObjectBehavior>().enabled = false;
            }
        }

        #endregion
    }
}
