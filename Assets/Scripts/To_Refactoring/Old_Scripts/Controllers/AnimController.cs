using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    public class AnimController : BaseController
    {
        /// <summary>
        /// Ссылка на аниматор
        /// </summary>
        private readonly Animator _animator;

        /// <summary>
        /// Ссылка на параметры анимаций
        /// </summary>
        public AnimationsParametorsModel AnimationsParametorsModel;

        /// <summary>
        /// Ссылка на модель игрока
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="player">Объект игрока</param>
        public AnimController(GameObject player)
        {
            this.player = player;
            AnimationsParametorsModel = new AnimationsParametorsModel();
            _animator = player.GetComponent<Animator>();
        }

        //Состояния передающиеся в параметры анимаций (Состояния анимаций)
        public bool Roll { get; private set; } /// Кувырок
        public bool Jump { get; private set; } /// Прыжок
        public bool Run { get; private set; } /// Бег
        public bool Defence { get; private set; } /// Защита
        public bool NormaAttack { get; private set; } ///Обычная атака
        public bool HeavyAttack { get; private set; } /// Сильная атака
        public bool Aiming { get; private set; } /// Прицеливание
		public bool isGrounded { get; private set; }
        public float Horizontal { get; private set; } 
        public float Vertical { get; private set; }
		//
		public bool Death { get; private set; }


        //Состояния проигрывания анимаций
        public bool NormalAttackInPlay = true;
        public bool HeavyAttackInPlay = true;

        public override void ControllerUpdate()
        {
            //Вызов метода проверки состояний игрока 
            GetInputs();

            //Вызов метода проигрывания анимаций игрока
            PlayAnimations();

			//Меняем параметр isGrounded
        	_animator.SetBool(AnimationsParametorsModel.isGrounded, isGrounded);
        }

        /// <summary>
        /// Метод Проигрывания Анимаций
        /// </summary>
        private void PlayAnimations()
        {
            //Горизонтальный ввод клавиатуры
            _animator.SetFloat(AnimationsParametorsModel.horizontal, Horizontal);

            //Вертикальный ввод клавиатуры
            _animator.SetFloat(AnimationsParametorsModel.vertical, Vertical);

            //Проверка на Прыжок 
            _animator.SetBool(AnimationsParametorsModel.isJumping, Jump);

            //Проверка на Бег
            _animator.SetBool(AnimationsParametorsModel.isRuning, Run);


            //Проверка на Кувырок 
            _animator.SetBool(AnimationsParametorsModel.isRolling, Roll);

            //Проверка на Обычный удар
            if (NormaAttack && NormalAttackInPlay)
            {
                _animator.SetBool(AnimationsParametorsModel.isNormalAttack, true);              
            }
            else if(!NormalAttackInPlay)
            {
                _animator.SetBool(AnimationsParametorsModel.isNormalAttack, false);             
            }
            

            //Проверка на Тяжелый удар
            if(HeavyAttack && HeavyAttackInPlay)
            {
				_animator.SetBool(AnimationsParametorsModel.isHeavyAttack, true);
            	StartScript.GetStartScript.InputController.HeavyAttackClick = false;            
            }
            else if(!HeavyAttackInPlay)
            {
            	_animator.SetBool(AnimationsParametorsModel.isHeavyAttack, false);
            }
            

            if (Defence)
            {
                _animator.SetBool(AnimationsParametorsModel.isDefence, true);
                Debug.Log("Defence");
            }
            else
            {
                _animator.SetBool(AnimationsParametorsModel.isDefence, false);
            }

            _animator.SetBool(AnimationsParametorsModel.isAiming, Aiming);

			//
			if (Death)
			{
				_animator.SetBool(AnimationsParametorsModel.isDeath, Death);
				Debug.Log("death");
			}
            else
            {
                _animator.SetBool(AnimationsParametorsModel.isDeath, Death);
            }
        }

        /// <summary>
        /// Метод проверки состояний различных состояний игрока
        /// </summary>
        private void GetInputs()
        {
            Horizontal = StartScript.GetStartScript.InputController.ForwardBackward;
            Vertical = StartScript.GetStartScript.InputController.LeftRight;
            Defence = StartScript.GetStartScript.InputController.Defence;
            Jump = StartScript.GetStartScript.StaminaController.CanJump;
            Run = StartScript.GetStartScript.StaminaController.CanRun;
            Roll = StartScript.GetStartScript.StaminaController.CanRoll;
            NormaAttack = StartScript.GetStartScript.StaminaController.CanNormalAttack;
            HeavyAttack = StartScript.GetStartScript.StaminaController.CanHeavyAttack;
            Aiming = StartScript.GetStartScript.InputController.Aim;			
        	isGrounded = StartScript.GetStartScript.MovementController.IsGrounded;
			//
			Death = StartScript.GetStartScript.HealthController.IsDeath;
          //  Debug.Log(NormalAttackInPlay);
		}
    }
}