using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Контроллер ввода
    /// </summary>
    internal class InputController : BaseController
    {
        public InputController()
        {
            //Временно решение
            //TODO: Исправить, или убрать
            PcInputModel = new PCInput();
        }

        #region Модель

        public PCInput PcInputModel { get; }

        #endregion

        public override void ControllerUpdate()
        {
            ForwardBackward = Input.GetAxis("Horizontal");

            LeftRight = Input.GetAxis("Vertical");

            Jump = Input.GetButtonDown("Jump");

            Run = Input.GetButton("Sprint");

            RotationY = Input.GetAxis("Mouse X");

            RotationX = -Input.GetAxis("Mouse Y");

            Aim = Input.GetButton("AimButton");

            LeftClickDown = Input.GetMouseButtonDown(0);

            LeftClickPressing = Input.GetMouseButton(0);

            LeftClickUp = Input.GetMouseButtonUp(0);

            Zoom = Input.GetAxis("Mouse ScrollWheel");

            Roll = Input.GetButton("Roll");

            Defence = Input.GetButton("Block");

            CameraCenter = Input.GetButton("CenterCamera");
			
			Inventory = Input.GetButtonDown("Inventory");

            #region Проверка на зажатие левой кнопки мыши для Тяжелой Атаки 2

            if (LeftClickPressing && !testFlagForHeavyAttack)
            {
                countOfHeavyAttackTimer += Time.deltaTime;
                if(countOfHeavyAttackTimer >= timerOfHeavyAttack)
                {                    
                    countOfHeavyAttackTimer = 0;
                    HeavyAttackClick = true;
                    testFlagForHeavyAttack = true;
                }
            }

            if (!LeftClickPressing)
            {
                countOfHeavyAttackTimer = 0f;
                HeavyAttackClick = false;
                testFlagForHeavyAttack = false;
            }
            
            #endregion

            #region Проверка на Двойной клик ЛКМ для тяжелой атаки

            //if (Input.GetMouseButtonUp((int)PCInputModel.LeftMouseButton))
            //{
            //    isLeftClickUp = true;
            //    countLeftClick++;
            //}

            //if (isLeftClickUp)
            //{
            //    countTimer += Time.deltaTime;
            //    if (countTimer >= timeToDoubleLeftClick)
            //    {
            //        countTimer = 0;
            //        isLeftClickUp = false;
            //        countLeftClick = 0;
            //        HeavyAttackClick = false;
            //    }
            //}

            //if (countLeftClick >= 2)
            //{
            //    HeavyAttackClick = true;
            //}

            #endregion
        }

        #region Клавиатура

        public float ForwardBackward { get; private set; }

        public float LeftRight { get; private set; }

        public bool Jump { get; private set; }

        public bool Run { get; private set; }

        public bool Roll { get; private set; }

        public bool Defence { get; private set; }

        public bool CameraCenter { get; private set; }
		
		public bool Inventory { get; private set; }

        #endregion

        #region Мышь

        public bool Aim { get; private set; }

        public bool LeftClickDown { get; private set; }

        public bool LeftClickPressing { get; private set; }

        public bool LeftClickUp { get; private set; }

        public bool HeavyAttackClick { get; set; }

        public float RotationY { get; private set; }

        public float RotationX { get; private set; }

        public float Zoom { get; private set; }

        #endregion

        #region Таймер Тяжелой Атаки 2

        public float timerOfHeavyAttack = 1f;
        public float countOfHeavyAttackTimer = 0f;
        public bool testFlagForHeavyAttack = false;

        #endregion

        #region Таймер Тяжелой Атаки

        public float timeToDoubleLeftClick = 1f;

        public float countTimer = 0f;

        public float countLeftClick = 0f;

        public bool isLeftClickUp = false;

        #endregion
    }
}