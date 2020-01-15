using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    internal class MovementController : BaseController
    {
        //Ссылка на камеру игрока
        private readonly Transform _camera;

        private readonly CharController _charController;

        //Ccылка на контроллер ввода
        private readonly InputController _inputController;

        //Ссылка на компонент Transform игрока
        private readonly Transform _player;

        #region Модель

        //Модель передвижения игрока
        private readonly PlayerMovement _playerMovement;

        #endregion

        /// <summary>
        ///     Кватернион для вращения персонажа в режиме прицеливания.
        /// </summary>
        private Quaternion _characterAimRotation;

        /// <summary>
        ///     Кватернион для направления движения.
        /// </summary>
        private Quaternion _direction;

        //Вектор движения персонажа
        private Vector3 _movement;

        /// <summary>
        ///     Структура для получения информации о столкновении луча с объектом.
        /// </summary>
        private RaycastHit _rayHit;

        private float _rotationY;

        /// <summary>
        ///     Переменная для задания скорости
        /// </summary>
        private float _speed;

        /// <summary>
        ///     Кватернион для сохранения вращения камеры.
        /// </summary>
        private Quaternion _tempCameraRotation;

        //Позиция персонажа по оси X
        private float _x;

        //Позиция персонажа по оси Y
        private float _y;

        //Позиция персонажа по оси Z
        private float _z;

        /// <summary>
        ///     Контроллер движения персонажа
        /// </summary>
        /// <param name="player">компонент Transform игрока</param>
        /// <param name="charController">Контроллер персонажа</param>
        public MovementController(Transform player, CharController charController)
        {
            //Cоздаем вектор движения
            _movement = new Vector3();

            //Получаем позицию игрока
            _player = player;

            //Получаем позицию камеры
            if (Camera.main != null) _camera = Camera.main.transform;

            //Создаем модель передвижения игрока
            _playerMovement = new PlayerMovement();

            // Контроллер персонажа
            _charController = charController;

            //Получаем ссылку на контроллер ввода.
            _inputController = StartScript.GetStartScript.InputController;
        }

        public override void ControllerUpdate()
        {
            
        }

        public override void ControllerFixedUpdate()
        {
            //Задаем нулевой вектор движения
            _movement = Vector3.zero;

            //Получаем данные с клавиатуры и мыши от контроллера ввода
            GetInputs();

            //Проверяем поверхность под игроком
            GroundCheck();

            //Проверяем стоит ли персонаж на месте
            IsStanding = IsGrounded & (_z == 0) & (_x == 0);

            IsWalking = IsGrounded & !IsRunning & (_z > 0 || _x > 0);

            //Прыжки и гравитация
            JumpAndGravity();

            //Передвижение
            CharacterMove();
        }

        public override void ControllerLateUpdate()
        {
            Roll();
        }

        /// <summary>
        ///     Получаем данные из контроллера ввода
        /// </summary>
        private void GetInputs()
        {
            _speed = _playerMovement.Speed;

            IsAiming = StartScript.GetStartScript.InputController.Aim;

            _rotationY = StartScript.GetStartScript.CameraController.YRotation;

            IsRunning = StartScript.GetStartScript.StaminaController.CanRun;

            IsRolling = StartScript.GetStartScript.StaminaController.CanRoll;

            IsDefencing = StartScript.GetStartScript.InputController.Defence;

            if (IsRunning & !IsAiming) _speed = _playerMovement.RunSpeed;
            if (IsDefencing & !IsAiming) _speed = _playerMovement.DefenceSpeed;

            // Check to see if the A or D key are being pressed
            _x = Input.GetAxis("Horizontal") * _speed;
            // Check to see if the W or S key is being pressed.  
            _z = Input.GetAxis("Vertical") * _speed;
        }

        /// <summary>
        ///     Метод для прыжка персонажа !!! На первое время, потом переделать !!!
        /// </summary>
        private void JumpAndGravity()
        {
            //Если игрок стоит на поверхности то можем прыгать.
            if (IsGrounded && StartScript.GetStartScript.StaminaController.CanJump)
                _charController.Rigidbody.AddForce(Vector3.up * _charController.JumpForce, ForceMode.VelocityChange);
            //else
            //    _charController.Rigidbody.AddForce(Vector3.down * _charController.charFallForce,
            //        ForceMode.VelocityChange);
        }

        /// <summary>
        ///     Метод перемещения игрока
        /// </summary>
        private void CharacterMove()
        {
            if (!_specialMove)
            {
                //Добавляем скорость движения. Изменяем положение по осям x и z
                _movement.x = _x;
                _movement.z = _z;
            }


            //Ограничиваем скорость движения по диагонали.
            _movement = Vector3.ClampMagnitude(_movement, _speed);

            //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
            _camera.eulerAngles = new Vector3(0, _camera.eulerAngles.y, 0);

            if (IsGrounded) _movement = _camera.TransformDirection(_movement);

            switch (IsAiming)
            {
                case true:

                    //Задаем направление вращения игрока как вращение камеры, если при прицеливанни игрок развернут в другую сторону
                    if (Quaternion.Dot(_player.rotation, _camera.rotation) < 0.9f) _player.rotation = _camera.rotation;


                    //Вращаем персонажа если она на поверхности.
                    if (IsGrounded & !_specialMove)
                        _player.rotation *= Quaternion.Euler(0,
                            _inputController.RotationY * _playerMovement.AimRotateSpeed * Time.deltaTime, 0);

                    break;

                case false:

                    if (!IsGrounded) _movement = _player.forward * _movement.magnitude;

                    //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
                    if (_movement != Vector3.zero) _direction = Quaternion.LookRotation(_movement);

                    //Вращаем персонажа если он двигается и он на поверхности
                    if ((_movement != Vector3.zero) & IsGrounded)
                        _player.rotation = Quaternion.Lerp(_player.rotation, _direction,
                            _playerMovement.RotateSpeed * Time.deltaTime);

                    break;
            }

            //Задаем направление по горизонтали
            _movement.y = _y;

            //Двигаем персонажа
            _charController.Move(_movement * Time.deltaTime);
        }

        /// <summary>
        ///     Метод проверки поверхности под игроком
        /// </summary>
        private void GroundCheck()
        {
            //RayHit = new RaycastHit();

            ////Делаем луч видимым
            //Debug.DrawRay(Player.transform.position + CharController.charCenter, (-CharController.transform.up * ((CharController.charHeight / 2) + PlayerMovement.GroundRayDistanceOffset)), Color.yellow);

            //if (Y < 0 & Physics.Raycast(Player.transform.position + CharController.charCenter, Vector3.down, out RayHit))
            //{
            //    IsGrounded = RayHit.distance < ((CharController.charHeight / 2) + PlayerMovement.GroundRayDistanceOffset);
            //}
            //else
            //{
            //    IsGrounded = false;
            //}

            IsGrounded = _charController.charIsGrounded;
        }

        /// <summary>
        ///     Метод кувырка
        /// </summary>
        private void Roll()
        {
            if (!IsRolling) return;
            if (!StartScript.GetStartScript.StaminaController.CanRoll) return;
            Debug.Log("Rolling");
            _charController.Rigidbody.AddForce(_player.forward * _charController.RollSpeed, ForceMode.Impulse);
            _charController.Rigidbody.AddForce(Vector3.down * _charController.FallForce,
                ForceMode.VelocityChange);
        }

       #region Флаги состояний

        public bool IsRunning { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsStanding { get; private set; }
        public bool IsRolling { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsAiming { get; private set; }
        public bool IsDefencing { get; private set; }

        private readonly bool _specialMove = false;

        #endregion
    }
}