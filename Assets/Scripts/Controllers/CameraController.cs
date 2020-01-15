using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    using UnityCamera = Camera;

    internal class CameraController : BaseController
    {
        /// <summary>
        /// Позиция камеры при прицеливании
        /// </summary>
        private readonly Transform _aimPosition;

        /// <summary>
        /// Флаг для возвращение камеры в стандартное положение за спину персонажа
        /// </summary>
        private bool _backToStandartCameraProsition;

        /// <summary>
        /// Флаг для центрирования камеры.
        /// </summary>
        private bool _cameraCenter;

        private Vector3 _cameraCenterPosition;

        #region Модель

        /// <summary>
        /// Модель камеры
        /// </summary>
        private readonly CameraModel _cameraModel;

        #endregion

        /// <summary>
        /// Флаг для наличия препятствий
        /// </summary>
        private bool _cameraObstacle;

        /// <summary>
        /// Ccылка на контроллер ввода
        /// </summary>
        private readonly InputController _inputController;

        /// <summary>
        /// Флаг для прицеливания
        /// </summary>
        private bool _isAiming;

        /// <summary>
        /// What is It?
        /// </summary>
        private bool _noInputsStartPosition = true;

        /// <summary>
        /// Расстояние до камеры с препятствием.
        /// </summary>
        private Vector3 _obstacleCameraPosition;

        /// <summary>
        /// Вектор расстояния между игроком и камерой.
        /// </summary>
        private Vector3 _offset;

        /// <summary>
        /// Положение игрока
        /// </summary>
        private readonly Transform _player;

        /// <summary>
        /// Луч для проверки препятствия перед камерой.
        /// </summary>
        private RaycastHit _ray;

        /// <summary>
        /// Кватернион для вращения
        /// </summary>
        private Quaternion _rotation;

        /// <summary>
        /// Угол вращения камеры по оси X.
        /// </summary>
        private float _rotationX;

        //Угол вращения камеры по оси Y.

        /// <summary>
        /// Позиция камеры без препятствий
        /// </summary>
        private Vector3 _standartCameraPosition;

        //Приближение камеры.
        private float _zoom;

        

        /// <summary>
        /// </summary>
        /// <param name="cameraModel">Модель камеры</param>
        /// <param name="player">Позиция игрока</param>
        /// <param name="camera">Ссылка на MainCamera</param>
        /// <param name="inputController">ссылка на контроллер ввода</param>
        public CameraController(CameraModel cameraModel, Transform player, UnityCamera camera,
            InputController inputController)
        {
            _aimPosition = GameObject.FindGameObjectWithTag("AimPosition").transform;

            //Получаем модель для камеры.
            _cameraModel = cameraModel;
            _player = player;
            this.Camera = camera;

            //Получаем ссылку на контроллер ввода
            _inputController = inputController;

            //Вычисляем стартовое положение камеры
            var transform = player.transform;
            var position = transform.position;
            var startCameraDistance = position +
                                          -Vector3.forward *
                                          (cameraModel.CameraMinDistance + cameraModel.CameraMaxDistance / 2);

            //Задаем начальное расстояние между камерой и игроком
            _offset = position - startCameraDistance;

            //Устанавливаем камеру в начальное положение
            camera.transform.position = position - transform.rotation * _offset;
        }

        /// <summary>
        /// Камера Unity
        /// </summary>
        public UnityCamera Camera { get; }

        /// <summary>
        /// Вращение по оси Y
        /// </summary>
        public float YRotation { get; private set; }

        /// <summary>
        ///     Update контроллера
        /// </summary>
        public override void ControllerLateUpdate()
        {
           
            //Получаем данные с клавиатуры и мыши.
            GetInputs();

            //Ограничиваем движение камеры по оси X
            _rotationX = _isAiming ? Mathf.Clamp(_rotationX, -45, 45) : Mathf.Clamp(_rotationX, 0, 70);

            if (!_noInputsStartPosition)
            {
                //Проверяем коллизию
                if (_isAiming)
                {
                    //Преобразуем угол Еулера в кватернион. Поворачиваем игрока в сторону мышью при прицеливании.
                    _rotation = Quaternion.Euler(_rotationX, _player.eulerAngles.y, 0);
                }
                else
                {
                    //Если не нужно возвращаться в стандартную позицию.
                    if (!_backToStandartCameraProsition) _rotation = Quaternion.Euler(_rotationX, YRotation, 0);
                }

                //Двигаем камеру
                CameraMove(_rotation);
            }

            else
            {
                var transform = _player.transform;
                CollisionCheck(transform.position, transform.rotation);
                if (_cameraObstacle) Camera.transform.position = _obstacleCameraPosition;
            }

            //Задаем направление "объектива" камеры, если игрок не прицеливается
            if (!_isAiming) Camera.transform.LookAt(_player.transform);

        }

        /// <summary>
        ///     Получаем данные из контроллера ввода
        /// </summary>
        private void GetInputs()
        {
            //Проверяем нажата ли кнопка прицеливания
            _isAiming = _inputController.Aim;

            //Получаем значения колесика мыши
            _zoom = _inputController.Zoom;

            //Получаем значения с кнопки центрирования камеры
            if (_inputController.CameraCenter) _cameraCenter = true;

            //Получаем значения движения мыши по оси X
            _rotationX += _inputController.RotationX * (_cameraModel.AxisY_MouseSensivity * 2);

            if (!_backToStandartCameraProsition)
                YRotation += _inputController.RotationY * (_cameraModel.AxisX_MouseSensivity * 2);
            else
                YRotation = Camera.transform.eulerAngles.y;

            if (_cameraCenter) YRotation = _player.eulerAngles.y;

            if (!(((_inputController.RotationX != 0) | (_inputController.RotationY != 0)) &
                  _noInputsStartPosition)) return;
            _noInputsStartPosition = false;

            YRotation = _player.transform.rotation.eulerAngles.y;
        }

           /// <summary>
           /// Метод для передвижения камеры
           /// </summary>
           /// <param name="rotation">Текущее врещение камеры</param>
        private void CameraMove(Quaternion rotation)
        {
            //TODO: Why Switches instead of Ifs?
            switch (_isAiming)
            {
                //Игрок в режиме прицеливания
                case true:

                    Transform transform;
                    (transform = Camera.transform).rotation = _player.transform.rotation * Quaternion.Euler(_rotationX, 0, 0);
                    Camera.transform.position = Vector3.Lerp(transform.position, _aimPosition.position,
                        _cameraModel.CameraMoveSpeed * Time.deltaTime);
                    _backToStandartCameraProsition = true;

                    break;

                case false:

                    //Проверяем наличие препятствий без центрирования камеры
                    if (!_cameraCenter)
                    {
                        //Проверка коллизии.
                        CollisionCheck(_player.transform.position, rotation);

                        switch (_cameraObstacle)
                        {
                            case true:

                                Camera.transform.position = Vector3.Lerp(Camera.transform.position,
                                    _obstacleCameraPosition, _cameraModel.CameraObstacleAvoidSpeed * Time.deltaTime);

                                if (_backToStandartCameraProsition &
                                    (Vector3.Distance(Camera.transform.position, _obstacleCameraPosition) < 0.2f))
                                    _backToStandartCameraProsition = false;

                                break;

                            case false:

                                //Получаем нужное положение для камеры
                                _standartCameraPosition = _player.transform.position - rotation * _offset;

                                //Задаем нужное положение для камеры
                                Camera.transform.position = Vector3.Lerp(Camera.transform.position,
                                    _standartCameraPosition, _cameraModel.CameraReturnSpeed * Time.deltaTime);

                                if (_backToStandartCameraProsition &
                                    (Vector3.Distance(Camera.transform.position, _standartCameraPosition) < 0.2f))
                                    _backToStandartCameraProsition = false;

                                break;
                        }
                    }

                    //Центрирование камеры
                    if (_cameraCenter)
                    {
                        //Проверка коллизии.
                        CollisionCheck(_player.transform.position, Quaternion.Euler(_rotationX, _player.eulerAngles.y, 0));

                        switch (_cameraObstacle)
                        {
                            case true:

                                Camera.transform.position = Vector3.Lerp(Camera.transform.position,
                                    _obstacleCameraPosition, _cameraModel.CameraObstacleAvoidSpeed * Time.deltaTime);

                                if (Vector3.Distance(Camera.transform.position, _obstacleCameraPosition) < 0.2f)
                                    _cameraCenter = false;

                                break;

                            case false:

                                _standartCameraPosition =
                                    _player.transform.position -
                                    Quaternion.Euler(_rotationX, _player.eulerAngles.y, 0) * _offset;

                                //Задаем нужное положение для камеры
                                Camera.transform.position = Vector3.Lerp(Camera.transform.position,
                                    _standartCameraPosition, _cameraModel.CameraReturnSpeed * Time.deltaTime);

                                if (Vector3.Distance(Camera.transform.position, _standartCameraPosition) < 0.2f)
                                    _cameraCenter = false;

                                break;
                        }
                    }

                    break;
            }

            //Управление зумом камеры.
            CameraZoom();
        }

        /// <summary>
        ///     Метод проверки коллизий
        /// </summary>
        /// <param name="playerPosition">Позиция игрока</param>
        /// <param name="rotation">Вращение камеры</param>
        private void CollisionCheck(Vector3 playerPosition, Quaternion rotation)
        {
            Debug.DrawLine(playerPosition, playerPosition - rotation * _offset, Color.yellow);

            //Проверяем столкновение луча с препятствием
            if (Physics.Linecast(playerPosition, playerPosition - rotation * _offset, out _ray, _cameraModel.Mask))
            {
                _cameraObstacle = true;

                //Отдялаем новую позицию камеры от препятствия.
                _obstacleCameraPosition = _ray.point + rotation * (Vector3.forward * _cameraModel.DistanceFromObstacle);

                Debug.DrawLine(_player.transform.position, _obstacleCameraPosition, Color.red);

                return;
            }

            _cameraObstacle = false;
        }

        /// <summary>
        ///     Метод "зума" камеры
        /// </summary>
        private void CameraZoom()
        {
            //Управление зумом камеры.
            if (_zoom == 0) return;
            //Муняем зум на колесо мыши
            _offset.z -= 1 * Input.GetAxis("Mouse ScrollWheel") * _cameraModel.CameraZoomSpeed * Time.deltaTime;

            //Ограничиваем зум камеры.
            switch (_cameraObstacle)
            {
                case true:
                    var maxObstacleDist = Vector3.Distance(_player.transform.position, Camera.transform.position);
                    _offset.z = Mathf.Clamp(_offset.z, _cameraModel.CameraMinDistance, maxObstacleDist);
                    break;

                case false:
                    _offset.z = Mathf.Clamp(_offset.z, _cameraModel.CameraMinDistance, _cameraModel.CameraMaxDistance);
                    break;
            }
        }
        public override void ControllerUpdate()
        {
        }
    }
}