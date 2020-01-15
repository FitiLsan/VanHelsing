using UnityEngine;

namespace Models.NPCScripts
{
    /// <summary>
    ///     Класс посредник для интерактивных объектов и персонажей
    /// </summary>
    public class Mediator : MonoBehaviour
    {
        public delegate void InteractContainer(bool cond);

        private bool active;
        public static event InteractContainer InteractEvent;

        private void Awake()
        {
            active = false;
        }

        public void Activate()
        {
            active = true;
            if (InteractEvent != null) InteractEvent(active);
        }

        public void Deactivate()
        {
            active = false;
            if (InteractEvent != null) InteractEvent(active);
        }
    }
}