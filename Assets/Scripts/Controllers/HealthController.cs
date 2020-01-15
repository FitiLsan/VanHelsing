using BaseScripts;
using Models;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Контроллер здоровья игрока
    /// </summary>
    public class HealthController : BaseController
    {
        private float _health;
        private readonly float _healthMaximum;
        private readonly HealthModel _healthModel;

		//смерть персонажа 
		public bool IsDeath = false;
		 
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="health">Ссылка на здоровье</param>
		/// <param name="healthModel">Модель здоровья</param>
		public HealthController(ref float health, HealthModel healthModel)
        {
            _healthModel = healthModel;
            _health = health;
            _healthMaximum = healthModel.healthMaximum;
        }
		public HealthController(){}

        public override void ControllerUpdate()
        {
            _health = _healthModel.health;
            HealthRegeneration(_healthModel.healthRegenerationRate);

            _health = Mathf.Clamp(_health, 0, _healthMaximum);

            _healthModel.health = _health;

			if (_health <= 0)
			{
				IsDeath = true;
				StartScript.GetStartScript.MovementController.Off();// когда нет жизни персонаж умирает и не должен двигаться 
			}
			else if(_health > 0)
			{
				IsDeath = false;
				StartScript.GetStartScript.MovementController.On();
			}

        }

        /// <summary>
        /// Метод восстановления здоровья
        /// </summary>
        /// <param name="reg">Восстановление здоровья в секунду</param>
        private void HealthRegeneration(float reg)
        {
			if (_health > 0)
			{
				_health += reg * Time.deltaTime;
			}
        }
    }
}