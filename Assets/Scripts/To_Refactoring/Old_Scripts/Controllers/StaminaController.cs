using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    internal class StaminaController : BaseController
    {      
        private readonly InputController _inputController;
        private readonly MovementController _movementController;
        private readonly AnimController _animController;

        private bool _isStanding;
        private bool _isWalking;

        private bool _normalAttackPress;
        private bool _heavyAttackPress;

        private bool _normalAttackInPlay;
        private bool _heavyAttackInPlay;

        private bool _jumpPress;
        private bool _rollPress;
        private bool _runPress;

        private float _stamina;
        private readonly float _staminaMaximum;
        private readonly StaminaModel _staminaModel;

        /// <summary>
        /// </summary>
        /// <param name="stamina">Ссылка на текущее значение стамины</param>
        /// <param name="staminaModel">Ссылка на модель характеристик игрока</param>
        /// <param name="inputController">Ссылка на контроллер ввода</param>
        /// <param name="movementController">Ссылка на контроллер перемещения</param>
        public StaminaController(ref float stamina, StaminaModel staminaModel,
            InputController inputController, MovementController movementController, AnimController animController)
        {
            //Получаем ссылки
            this._stamina = stamina;

            this._staminaModel = staminaModel;

            this._movementController = movementController;

            this._inputController = inputController;

            this._animController = animController;

            _staminaMaximum = staminaModel.StaminaMaximum;
        }

        /// <summary>
        /// Можем ли бежать
        /// </summary>
        public bool CanRun { get; private set; }
        /// <summary>
        /// Можем ли прыгать
        /// </summary>
        public bool CanJump { get; private set; }
        /// <summary>
        /// Можем ли кувыркаться
        /// </summary>
        public bool CanRoll { get; private set; }
        /// <summary>
        /// Можем ли атаковать нормальной атакой
        /// </summary>
        public bool CanNormalAttack { get; private set; }
        /// <summary>
        /// Можем ли атаковать сильной атакой
        /// </summary>
        public bool CanHeavyAttack { get; private set; }


        /// <summary>
        /// Получение вводов и флагов
        /// </summary>
        private void GetInputsAndFlags()
        {
            _runPress = _inputController.Run;
            _jumpPress = _inputController.Jump;
            _rollPress = _inputController.Roll;
            _normalAttackPress = _inputController.LeftClickUp;
            _heavyAttackPress = _inputController.HeavyAttackClick;
            _isStanding = _movementController.IsStanding;
            _isWalking = _movementController.IsWalking;
            _normalAttackInPlay = _animController.NormalAttackInPlay;
            _heavyAttackInPlay = _animController.HeavyAttackInPlay;
        }


        public override void ControllerUpdate()
        {
            GetInputsAndFlags();

            if (_isStanding) Regenerate(_staminaModel.StaminaStandRegenRate);
            if (_isWalking) Regenerate(_staminaModel.StaminaWalkRegenRate);

            RunStaminaDrain();
            JumpStaminaDrain();
            RollStaminaDrain();
            NormalAttackStaminaDrain();
            HeavyAttackStaminaDrain();


            //Ограничиваем значения стамины
            _stamina = Mathf.Clamp(_stamina, 0, _staminaMaximum);
            _staminaModel.Stamina = _stamina;
        }

        /// <summary>
        ///     Метод регенерации стамины
        /// </summary>
        private void Regenerate(float reg)
        {
            _stamina += reg * Time.deltaTime;
        }

        /// <summary>
        ///     Метод расхода стамины на прыжок
        /// </summary>
        private void JumpStaminaDrain()
        {
            CanJump = _jumpPress & (_stamina > _staminaModel.StaminaJumpCoast);

            if (CanJump & _movementController.IsGrounded) _stamina -= _staminaModel.StaminaJumpCoast;
        }

        /// <summary>
        ///     Метод расхода стамины на бег
        /// </summary>
        private void RunStaminaDrain()
        {
            CanRun = _runPress & _movementController.IsGrounded & (_stamina > 0);

            if (CanRun) _stamina -= _staminaModel.StaminaRunCoast * Time.deltaTime;
        }

        /// <summary>
        ///     Метод расхода стамины на кувырок
        /// </summary>
        private void RollStaminaDrain()
        {
            CanRoll = _rollPress & _movementController.IsGrounded & (_stamina > _staminaModel.StaminaRollCoast);

            if (CanRoll) _stamina -= _staminaModel.StaminaRollCoast;
        }

        /// <summary>
        ///     Метод расхода стамины на Обычную Атаку
        /// </summary>
        private void NormalAttackStaminaDrain()
        {
            CanNormalAttack = _normalAttackPress & (_stamina > _staminaModel.StaminaNormalAttackCoast);

            if (CanNormalAttack && _normalAttackInPlay) _stamina -= _staminaModel.StaminaNormalAttackCoast;
        }

        /// <summary>
        ///     Метод расхода стамины на Тяжелую Атаку
        /// </summary>
        private void HeavyAttackStaminaDrain()
        {
            CanHeavyAttack = _heavyAttackPress & (_stamina > _staminaModel.StaminaNormalAttackCoast);

            if (CanHeavyAttack && _heavyAttackInPlay) _stamina -= _staminaModel.StaminaHeavyAttackCoast;
        }
    }
}