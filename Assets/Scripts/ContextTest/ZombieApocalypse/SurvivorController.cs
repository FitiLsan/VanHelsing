using UnityEngine;


namespace BeastHunter
{
    public sealed class SurvivorController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;
        private float _targetDistance;

        #endregion


        #region ClassLifeCycle

        public SurvivorController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            var zombiePosition = _context.ZombieModel.Transform.position;
            var survivorPosition = _context.SurvivorModel.Transform.position;

            var distance = Vector3.Distance(zombiePosition, survivorPosition);

            if (distance < _targetDistance)
            {
                var targetDirection = Vector3.MoveTowards(survivorPosition, zombiePosition, - _targetDistance * Time.deltaTime);

                _context.SurvivorModel.TargetDirection = targetDirection;
            }
            else
            {
                _context.SurvivorModel.TargetDirection = survivorPosition;
            }
            
            _context.SurvivorModel.Execute();
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _context.SurvivorModel.Behavior.OnFilterHandler += OnFilterHandler;
            _context.SurvivorModel.Behavior.OnTriggerEnterHandler += OnTriggerEnterHandler;

            _targetDistance = _context.SurvivorModel.TestCharacterStruct.TargetDistance;
        }

        #endregion
        

        #region ITearDown

        public void TearDown()
        {
            _context.SurvivorModel.Behavior.OnFilterHandler -= OnFilterHandler;
            _context.SurvivorModel.Behavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
        }

        #endregion


        #region Methods

        private void OnTriggerEnterHandler(ITrigger trigger, Collider other)
        {
            _context.SurvivorModel.KillSurvivor();
        }

        private bool OnFilterHandler(Collider other)
        {
            return other.CompareTag(TagManager.ENEMY);
        }

        #endregion
    }
}