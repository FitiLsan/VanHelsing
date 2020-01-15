using System.Collections.Generic;
using Controllers;
using DatabaseWrapper;
using Interfaces;
using Items;
using Models;
using Quests;
using UnityEngine;

namespace BaseScripts
{
    /// <summary>
    /// Основной класс игры. Управляет всеми контроллерами. Синглтон.
    /// </summary>
    internal class StartScript : MonoBehaviour
    {
        /// <summary>
        /// Прослойка, обеспечивающая работу с предметами в рамках базы мира и файла сейва
        /// </summary>
        private IItemStorage _itemStorage;

        /// <summary>
        /// Прослойка, обеспечивающая работу с квестами в рамках базы мира и файла сейва
        /// </summary>
        private IQuestStorage _questStorage;

        /// <summary>
        /// Позволяет обратиться к публичным api класса
        /// </summary>
        public static StartScript GetStartScript { get; private set; }

        private void Awake()
        {
            GetStartScript = this;

            //Init non-unity systems
            QuestRepository.Init();
            ItemTemplateRepository.Init();

            _questStorage = new DbQuestStorage();
            _itemStorage = new DbItemStorage();

            //Get objects
            var Player = GameObject.FindGameObjectWithTag("Player");
            var CameraCenter = GameObject.FindGameObjectWithTag("CameraCenter").transform;
            var PlayerAnimator = GameObject.FindGameObjectWithTag("PlayerAnimator");

            //Создаем контроллеры
            AnimController = new AnimController(PlayerAnimator);
            InputController = new InputController();
            CameraController = new CameraController(Camera.main.GetComponent<CameraModel>(), CameraCenter, Camera.main,
                InputController);
            MovementController = new MovementController(Player.transform, Player.GetComponent<CharController>());
            StaminaController = new StaminaController(ref Player.GetComponent<StaminaModel>().Stamina,
                Player.GetComponent<StaminaModel>(), InputController, MovementController, AnimController);          
            // enemyAttackController = new EnemyAttackController(targetDetector);
            HealthController = new HealthController(ref Player.GetComponent<HealthModel>().health,
                Player.GetComponent<HealthModel>());
            QuestLogController = new QuestLogController(_questStorage);
            InventoryController = new InventoryController(_itemStorage);

            //Находим необходимые контроллеры которые висят на объектах
            SwordStartController = FindObjectOfType<SwordStartController>();

            #region Добавляем контроллеры в коллекцию

            _allControllers.Add(InputController);
            _allControllers.Add(CameraController);
            _allControllers.Add(MovementController);
            _allControllers.Add(StaminaController);
            _allControllers.Add(AnimController);
            // AllControllers.Add(enemyAttackController);
            _allControllers.Add(HealthController);
            _allControllers.Add(QuestLogController);
            _allControllers.Add(InventoryController);

            #endregion
        }

        /// <summary>
        /// Выполняется каждый кадр
        /// </summary>
        private void Update()
        {
            //Запускаем Update каждого контроллера
            foreach (var controller in _allControllers)
                if (controller.IsActive)
                    controller.ControllerUpdate();
        }

        private void FixedUpdate()
        {
            foreach(var controller in _allControllers)
            {
                if (controller.IsActive)
                    controller.ControllerFixedUpdate();
            }
        }

        /// <summary>
        /// Физику обрабатывать здесь
        /// </summary>

        private void LateUpdate()
        {
            CameraController.ControllerLateUpdate();
            MovementController.ControllerLateUpdate();
        }

        #region Список контроллеров

        /// <summary>
        /// Контроллер Камеры
        /// </summary>
        public CameraController CameraController { get; private set; }

        /// <summary>
        /// Контроллер пользовательского ввода
        /// </summary>
        public InputController InputController { get; private set; }

        /// <summary>
        /// Контроллер передвижения персонажа
        /// </summary>
        public MovementController MovementController { get; private set; }

        /// <summary>
        /// Контроллер выносливости персонажа
        /// </summary>
        public StaminaController StaminaController { get; private set; }

        /// <summary>
        /// Контроллер анимаций
        /// </summary>
        public AnimController AnimController { get; private set; }

        //public EnemyAttackController enemyAttackController { get; private set; }

        /// <summary>
        /// Контроллер здоровья
        /// </summary>
        public HealthController HealthController { get; private set; }

        /// <summary>
        /// Контроллер квестов
        /// </summary>
        public QuestLogController QuestLogController { get; private set; }

        /// <summary>
        /// Контроллер инфентаря и экипировки
        /// </summary>
        public InventoryController InventoryController { get; private set; }

        /// <summary>
        /// Контроллер стартового меча
        /// </summary>
        public SwordStartController SwordStartController { get; private set; }

        /// <summary>
        /// Коллекция контроллеров
        /// </summary>
        private readonly List<BaseController> _allControllers = new List<BaseController>(6);

        #endregion
    }
}