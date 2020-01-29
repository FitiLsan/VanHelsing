using UnityEngine;
namespace BaseScripts
{
    /// <summary>
    /// Абстрактный класс, от которого наследуются все не MonoBehavior контроллеры
    /// </summary>
    public abstract class BaseController : BaseEntity, ITick, IAwake
    {
        /// <summary>
        /// Состояние контроллера (вкл/выкл)
        /// </summary>
        public bool IsActive { get; private set; } = true;
        
        /// <summary>
        ///     Включаем контроллер
        /// </summary>
        public virtual void On()
        {
            IsActive = true;
            
        }

        /// <summary>
        ///     Выключаем контроллер
        /// </summary>
        public virtual void Off()
        {
            IsActive = false;
        }

        /// <summary>
        ///     Переключаем контроллер
        /// </summary>
        public virtual void Switch()
        {
            IsActive = !IsActive;
        }
        
        /// <summary>
        /// Аналогичен MonoBehaviour.Update
        /// </summary>
        public virtual void ControllerUpdate() { }
        public virtual void ControllerFixedUpdate() { }
        public virtual void ControllerLateUpdate() { }

        //Перейти контроллерам на ITick Update
        public virtual void Tick()
        {

        }
        public virtual void OnAwake()
        {

        }
    }
}