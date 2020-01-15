using System.Collections.Generic;
using Models.ConditionsAndActions.Helpers;
using Models.ConditionsAndActions.Helpers.Components;
using Models.NPCScripts.Enemy;
using UnityEngine;

namespace Models.ConditionsAndActions
{
    /// <summary>
    ///     Содержит информацию о доступных персонажу статусах и отслеживает их выполнение
    /// </summary>
    public class BaseConditions
    {
        /// <summary>
        ///     Содержит все методы состояний
        /// </summary>
        private readonly Dictionary<string, ConditionsUpdate> ConditionsMethods;

        /// <summary>
        ///     Содержит все характеристики состояний персонажа
        /// </summary>
        private readonly CharacterConditionsSpecifications ConditionsSpecifications;

        /// <summary>
        ///     Содержит таймеры для всех состояний
        /// </summary>
        private readonly Dictionary<string, float> ConditionTimers;

        public BaseConditions(List<string> ConditionsList, CharacterConditionsSpecifications ConditionsSpecifications)
        {
            #region Добавляем все состояния в коллекцию

            Conditions = new ConditionsCollection(ConditionsList)
            {
                new Condition("Alive", true)
            };

            Conditions.SetPropertyEventMethod(ConditionStatusHasChanged);

            #endregion

            //Получаем ссылку на характеристики состояний
            this.ConditionsSpecifications = ConditionsSpecifications;

            #region Заполняем словарь с методами состояний

            ConditionsMethods = new Dictionary<string, ConditionsUpdate>();

            ConditionsMethods.Add("Bleeding", Bleed);
            ConditionsMethods.Add("Slowed", Slowing);

            #endregion

            #region Заполняем словарь с таймерами

            ConditionTimers = new Dictionary<string, float>();

            ConditionTimers.Add("Bleeding", BleedingTimer);
            ConditionTimers.Add("Slowed", SlowingTimer);

            #endregion
        }

        /// <summary>
        ///     Коллекция содержащая все доступные персонажу статусы
        /// </summary>
        public ConditionsCollection Conditions { get; private set; }

        #region Запуск состояний

        /// <summary>
        ///     Запускает выполнение всех активных статусов
        /// </summary>
        /// <param name="spec">Модель данных противника</param>
        /// <param name="deltaTime">Время</param>
        public void ConditionsUpdateStart(BaseCharacterModel CharacterModel,
            ref EnemySpecifications enemySpecifications, float deltaTime)
        {
            ConditionsUpdateEvent?.Invoke(CharacterModel, ref enemySpecifications, deltaTime);
        }

        #endregion

        #region События

        /// <summary>
        ///     Событие запускающее активные статусы
        /// </summary>
        protected internal event ConditionsUpdate ConditionsUpdateEvent;

        /// <summary>
        ///     Свойство показывает, если на персонаже активные статусы требующие апдейта
        /// </summary>
        public bool HasActiveConditions { get; private set; }

        #endregion

        #region Таймеры

        private readonly float BleedingTimer = 0f;
        private readonly float SlowingTimer = 0f;

        #endregion

        #region Логика Состояний

        /// <summary>
        ///     Кровотечение
        /// </summary>
        /// <param name="CharacterModel">Модель персонажа</param>
        /// <param name="deltaTime">Время</param>
        public void Bleed(BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications,
            float deltaTime)
        {
            ConditionTimers["Bleeding"] += deltaTime;
            CharacterModel.Health -= ConditionsSpecifications.BleedingDamage;

            Debug.Log(
                $"!!!BLEED!!! Health:{CharacterModel.Health.ToString("0")} Condition Time Left: {(ConditionsSpecifications.BleedingTime - ConditionTimers["Bleeding"]).ToString("0.0")}");

            if (ConditionTimers["Bleeding"] >= ConditionsSpecifications.BleedingTime)
            {
                ConditionTimers["Bleeding"] = 0;
                Conditions.ChangeConditionStatus("Bleeding", false);
                ConditionsUpdateEvent -= Bleed;
            }
        }

        /// <summary>
        ///     Замедление
        /// </summary>
        /// <param name="CharacterModel">Модель персонажа</param>
        /// <param name="deltaTime">Время</param>
        public void Slowing(BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications,
            float deltaTime)
        {
            ConditionTimers["Slowed"] += deltaTime;
            CharacterModel.Speed = ConditionsSpecifications.SlowSpeed;
            CharacterModel.RunSpeed = ConditionsSpecifications.SlowSpeed;

            //Debug.Log(
            //    $"!!!SLOW!!! Condition Time Left: {(ConditionsSpecifications.SlowingTime - ConditionTimers["Slowed"]).ToString("0.0")}");

            if (ConditionTimers["Slowed"] >= ConditionsSpecifications.SlowingTime)
            {
                ConditionTimers["Slowed"] = 0;
                Conditions.ChangeConditionStatus("Slowed", false);
                ConditionsUpdateEvent -= Slowing;

                CharacterModel.Speed = enemySpecifications.Speed;
                CharacterModel.RunSpeed = enemySpecifications.RunSpeed;
            }
        }

        #region TO DO 

        //public void Immobilizing(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Blinding(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void KnokingDown(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Stuning(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Poison(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Weak(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        #endregion

        #endregion

        #region Изменение статуса состояний

        /// <summary>
        ///     Вызывается каждый раз при изменении статуса состояния
        /// </summary>
        /// <param name="Args">Содержит информацию об изменившемся статусе</param>
        private void ConditionStatusHasChanged(ConditionArgs Args)
        {
            if ((Args.ConditionName == "Alive") & (Args.ConditionStatus == false))
            {
                CharacterDeathStatus();
            }

            else
            {
                if (Args.ConditionStatus == false)
                {
                    ActiveConditionsCheck();
                }

                else
                {
                    if (!Conditions.CurrentConditionStatus(Args.ConditionName))
                        ConditionsUpdateEvent += ConditionsMethods[Args.ConditionName];
                    else
                        ConditionTimers[Args.ConditionName] = 0;

                    ActiveConditionsCheck();
                }
            }
        }

        /// <summary>
        ///     Проверяет активные статусы на персонаже
        /// </summary>
        private void ActiveConditionsCheck()
        {
            for (var i = 1; i < Conditions.Count; i++)
                if (Conditions[i].StatusChanged)
                {
                    HasActiveConditions = true;
                    return;
                }

            HasActiveConditions = false;
        }

        /// <summary>
        ///     В случае смерти персонажа отписываем от всех методов, меняем коллекцию статусов
        /// </summary>
        private void CharacterDeathStatus()
        {
            if (ConditionsUpdateEvent != null)
                foreach (var item in ConditionsUpdateEvent.GetInvocationList())
                    ConditionsUpdateEvent -= item as ConditionsUpdate;

            Conditions = new ConditionsCollection
            {
                new Condition("Alive", false)
            };
        }

        #endregion
    }
}