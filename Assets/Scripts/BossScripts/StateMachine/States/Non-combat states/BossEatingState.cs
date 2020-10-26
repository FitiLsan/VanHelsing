using UnityEngine;


namespace BeastHunter
{
    public class BossEatingState : BossBaseState
    {

        #region Constants

        private const float EATING_TIME = 5f;
        private const float DISTANCE_TO_START_EATING = 1.5f;
        private const float FOOD_POINT = 50f;

        #endregion


        #region Fields

        private float _eatingCountTime = EATING_TIME;
        private Vector3 _target;
        private bool _canEat;

        #endregion


        #region ClassLifeCycle

        public BossEatingState(BossStateMachine stateMachine) : base(stateMachine)
        {

        }

        #endregion


        #region Methods 

        public override void OnAwake()
        {
            CanExit = true;
            CanBeOverriden = true;
            IsBattleState = false;
        }

        public override void Initialise()
        {
            _stateMachine._model.BossNavAgent.speed = _stateMachine._model.BossData._bossSettings.WalkSpeed;
            _stateMachine._model.BossNavAgent.stoppingDistance = DISTANCE_TO_START_EATING;
            _stateMachine._model.BossAnimator.Play("MovingState");
            _canEat = false;
        }

        public override void Execute()
        {
            CheckTarget();
            CheckDistance();
            Eating();
        }

        public override void OnExit()
        {

        }

        public override void OnTearDown()
        {
        }


        private void CheckTarget()
        {
            _target = _stateMachine._model.FoodList[0].transform.position;
        }


        private void CheckDistance()
        {
            if (Mathf.Sqrt((_stateMachine._model.BossTransform.position - _target)
                .sqrMagnitude) <= DISTANCE_TO_START_EATING)
            {
                _canEat = true;
            }
            else
            {
                MoveToFood();
                _canEat = false;
            }
        }

        private void Eating()
        {
            if (_canEat)
            {
                Debug.Log("Eating");
                _stateMachine._model.BossAnimator.Play("EatingState");
                _eatingCountTime -= Time.deltaTime;
                if(_eatingCountTime <= 0)
                {
                    _eatingCountTime = EATING_TIME;
                    _stateMachine._model.FoodList[0].SetActive(false);
                    _stateMachine._model.FoodList.RemoveAt(0);
                    _stateMachine._model.CurrentStamina += FOOD_POINT;
                    _stateMachine.SetCurrentState(BossStatesEnum.Idle);
                }
            }
        }

        private void MoveToFood()
        {
            _stateMachine._model.BossData.MoveTo(_stateMachine._model.BossNavAgent, _target, _stateMachine._model.BossData._bossSettings.WalkSpeed);
        }

        #endregion
    }
}