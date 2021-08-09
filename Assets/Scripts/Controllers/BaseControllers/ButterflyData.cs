using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewButterflyModel", menuName = "CreateData/Butterfly", order = 2)]
    public sealed class ButterflyData : EnemyData
    {
        #region Fields

        public ButterflyStats ButterflyStats;
        public BehaviourStateButterfly BehaviourStateButterfly;

        [SerializeField, Range(0,10), Header("Время смены состояния")] private float _timeOfTheStateChange;

        private PhysicsService _physicsService;

        #endregion


        #region ClassLifeCycles

        public void OnEnable()
        {
            _physicsService = Services.SharedInstance.PhysicsService;
        }

        public void Act(ButterflyModel butterflyModel)
        {
            //TODO - почему тут каждый кадр слетает _physicsService?!?!?!?!?!?
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            switch (butterflyModel.ButterflyState)
            {
                case BehaviourStateButterfly.Idle:
                    Idle(butterflyModel);
                    break;
                case BehaviourStateButterfly.Fly:
                    Fly(butterflyModel);
                    break;
                default:
                    break;
            }
        }

        private void Idle(ButterflyModel butterflyModel)
        {
            Debug.Log($"Бабочка сидит на месте");
            StartTimerToChangeState(butterflyModel, BehaviourStateButterfly.Fly);
        }

        private void Fly(ButterflyModel butterfly)
        {
            Debug.Log($"Бабочка взлетает");
            StartTimerToChangeState(butterfly, BehaviourStateButterfly.Idle);
        }

        private void StartTimerToChangeState(ButterflyModel butterfly, BehaviourStateButterfly state)
        {
            butterfly.TimeSinceTheState += Time.deltaTime;
            if (butterfly.TimeSinceTheState >= _timeOfTheStateChange)
            {
                Debug.Log($"Меняем состояние");
                butterfly.TimeSinceTheState = 0.0f;
                butterfly.ButterflyState = state;
            }
        }

        #endregion
    }
}