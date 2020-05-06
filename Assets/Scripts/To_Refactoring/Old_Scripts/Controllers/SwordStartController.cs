using BaseScripts;
using Interfaces;
using Models.ConditionsAndActions;
using UnityEngine;

namespace Controllers
{
    public class SwordStartController : MonoBehaviour
    {
        //Текущий урон
        private float currentDamage;

        //Тяжелый урон
        private readonly float damageHeavy = 30f;

        //Обычный урон 
        private readonly float damageNormal = 10f;

        //Флаг атаки
        private readonly bool isAttack = true;

        //Флаг тяжелой атаки
        private bool isHeavyAttack;

        //Флаг обычной атаки
        private bool isNormalAttack;

        //Коллайдер
        public Collider swordCollider;

        private void Start()
        {
            swordCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider collider)
        {
            //Если коллайдер меча соприкоснулся с игроком то возвращаемся
            if (collider.tag == "Player") return;

            //Если коллайдер меча коснулся любого объекта то проверяем статус атаки 
            GetAttackStatus();

            //Вызываем метод Нанесения урона у всех объектов кто наследует интерфейс Нанесения урона
            SetDamage(collider.GetComponent<ISetDamage>());
            Debug.Log("Hit Enemy");
            swordCollider.enabled = false;

            //Вызываем метод передачи Статусов оружия
            SetConditions(collider.GetComponent<IGetConditions>());
            
        }       

        //Метод Нанесения урона
        private void SetDamage(ISetDamage obj)
        {
            //Проверка отсутствия объекта 
            if (obj != null) obj.ApplyDamage(currentDamage);
        }

        //Метод проверки статуса атаки
        private void GetAttackStatus()
        {
            //Состояние параметра обычной атаки у Аниматора
            isNormalAttack = StartScript.GetStartScript.AnimController.NormaAttack;

            //Состояние параметра тяжелой атаки у Аниматора
            isHeavyAttack = StartScript.GetStartScript.AnimController.HeavyAttack;

            //Если производится Обычная атака то
            if (isAttack) currentDamage = damageNormal;
            //Если производится Тяжелая атака то
            if (isAttack) currentDamage = damageHeavy;
        }

        #region Модификация

        [SerializeField] private ItemConditionsChance ItemConditions;

        private void Awake()
        {
            if (ItemConditions != null) ItemConditions.SetItemConditionsChance();
        }

        //Метод Передачи статуса
        private void SetConditions(IGetConditions obj)
        {
            //Проверка отсутствия объекта 
            if (obj != null)
                if (ItemConditions.GetCurrentItemConditions().Length != 0)
                    obj.ApplyCondition(ItemConditions.GetCurrentItemConditions());
        }

        #endregion
    }
}