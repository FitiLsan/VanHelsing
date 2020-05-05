using System;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models.NPCScripts.Enemy
{
    public class EnemyFightController
    {
        /// <summary>
        ///     Делегат для перехода в состояние погони
        /// </summary>
        /// <param name="unitName"></param>
        public delegate void AttackToChase(string unitName);

        private readonly float alternativeDistance; //Альтернативная дистанция атаки - для альтернативного режима атаки
        private float boostSpeed; //Скорость рывка - используется для быстрого сокращения/разрыва дистанции
        private bool choseAim; //Флаг состояния выбора цели
        private float currentAttackDistance; //текущая дистанция атаки
        private readonly float effectsDisplayTime = 0.1f; //Время действия визуальных эффектов
        private readonly Transform enemyTransform; //Локальная копия трансформа врага
        private readonly MeshRenderer gun; //МешРендерер стрелкового оружия
        private readonly Transform gunBarrelEnd; //Трансформ точки стрельбы
        private readonly AudioSource gunShotSound; //Звук стрельбы
        private RaycastHit hit; //Точка попадания луча
        private float hitChance; //Шанс попасть при стрельбе
        private int hitCount = 1;
        private readonly float hitSpeed; //Скорость ближнего боя
        private readonly MeshRenderer knife; //МешРендерер оружия ближнего боя
        private float meleeDamage; //Урон ближнего боя
        private int meleeHitCount; //счетчик нанесенных ударов ближнего боя - для спец способности
        private float missChance; //Шанс промахнуться при стрельбе

        /// <summary>
        ///     Локальные копии задействованных компонентов
        /// </summary>
        private readonly EnemyMove move; //Локальная копия класса движения движения

        /// <summary>
        ///     Локальные переменные, хранящие в себе состояния и значения параметров
        /// </summary>
        private readonly float
            priorityDistance; //Приоритетная дистанция атаки - выбирается автоматически при создании объекта, исходя из его типа

        private readonly float rangeAccuracy; //Дистанционная меткость
        private readonly float rangeDamage; //Дистанционный урон
        private readonly float reaction = 0.2f; //время принятия решения (между выбором цели и выстрелом)
        private Vector3 reactionAim; //точка, в которую будет произведен выстрел
        private float reactionTimer; //таймер принятия решения врагом
        private float runSpeed; //Скорость бега
        private readonly LineRenderer shootLine; //Визуальное отображение выстрела
        private Ray shootRay; //Луч для проверки попадания по объекту
        private readonly float shootSpeed; //Скорость стрельбы

        public
            /// <summary>
            /// Вспомогательные переменные для отслеживания меткости стрельбы
            /// </summary>
            int shotCount = 1;

        private bool specialAbility; //флаг включения\отключения спец способности
        private readonly float switchDistance; //Дистанция на которой происходит переключение режимов атаки
        private bool switchMode = true; //режим атаки (true - приоритетный, false - альтернативный)
        private float timer; //Основной задейсвованный таймер (Скорость стрельбы)

        /// <summary>
        ///     Конструктор класса
        /// </summary>
        /// <param name="move"></param>
        /// <param name="enemyTransform"></param>
        /// <param name="gun"></param>
        /// <param name="knife"></param>
        /// <param name="gunBarrelEnd"></param>
        /// <param name="shootLine"></param>
        /// <param name="priorityDistance"></param>
        /// <param name="alternativeDistance"></param>
        /// <param name="runSpeed"></param>
        /// <param name="rangeDamage"></param>
        /// <param name="rangeAccuracy"></param>
        /// <param name="shootSpeed"></param>
        /// <param name="meleeDamage"></param>
        /// <param name="hitSpeed"></param>
        /// <param name="gunShotSound"></param>
        public EnemyFightController(EnemyMove move, Transform enemyTransform, MeshRenderer gun, MeshRenderer knife,
            Transform gunBarrelEnd, LineRenderer shootLine, float priorityDistance, float alternativeDistance,
            float rangeDamage, float rangeAccuracy, float shootSpeed, float meleeDamage, float hitSpeed,
            AudioSource gunShotSound)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.shootLine = shootLine;
            this.priorityDistance = priorityDistance;
            this.alternativeDistance = alternativeDistance;
            switchDistance = Mathf.Abs(priorityDistance - alternativeDistance) / 2;
            currentAttackDistance = priorityDistance;
            this.rangeDamage = rangeDamage;
            this.rangeAccuracy = rangeAccuracy;
            this.shootSpeed = shootSpeed;
            this.gun = gun;
            this.knife = knife;
            this.gunBarrelEnd = gunBarrelEnd;
            this.gunShotSound = gunShotSound;
            this.meleeDamage = meleeDamage;
            this.hitSpeed = hitSpeed;
            hitChance = rangeAccuracy;
            missChance = 10 - rangeAccuracy;
        }

        public static event AttackToChase AttackToChaseEvent;

        /// <summary>
        ///     Основной вызываемый из контроллера врага метод - отвечает за режим боя
        /// </summary>
        /// <param name="archrival"></param>
        /// <param name="deltaTime"></param>
        public void Fight(GameObject archrival, float deltaTime)
        {
            timer += deltaTime;
            reactionTimer += deltaTime;
            SpecialAbilityActivator();
            if (timer > effectsDisplayTime)
                DisableEffects();
            if (specialAbility)
            {
                SpecialAbility(archrival.transform.position);
            }
            else
            {
                //Рассчет расстояния до потивника
                var distance = Mathf.Sqrt(Mathf.Pow(archrival.transform.position.x - enemyTransform.position.x, 2) +
                                          Mathf.Pow(archrival.transform.position.y - enemyTransform.position.y, 2) +
                                          Mathf.Pow(archrival.transform.position.z - enemyTransform.position.z, 2));
                if (distance > currentAttackDistance)
                {
                    //Debug.Log("MoveToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, Speed.Walk);
                    move.Rotate(RotateDirection(archrival.transform.position));
                    if (timer > 15f) AttackToChaseEvent(enemyTransform.name);
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        //Debug.Log("MoveToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, Speed.Run);
                        move.Rotate(RotateDirection(archrival.transform.position));
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        //Debug.Log("ThrowToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, Speed.Throw);
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (distance >= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = priorityDistance;
                        }
                    }
                }
                else if (distance <= currentAttackDistance)
                {
                    move.Stop();
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= shootSpeed)
                        {
                            if (choseAim)
                            {
                                if (reactionTimer > reaction)
                                {
                                    RangeAttack(reactionAim);
                                    choseAim = false;
                                }
                            }
                            else
                            {
                                //Выбор цели и запуск таймера реакции
                                reactionTimer = 0f;
                                reactionAim = ShootDirection(archrival.transform.position, rangeAccuracy);
                                choseAim = true;
                            }
                        }

                        if (distance <= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = alternativeDistance;
                        }
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= hitSpeed)
                        {
                            MeleeAttack();
                            meleeHitCount++;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Метод выбора направления для вращения врага
        /// </summary>
        /// <param name="archrival"></param>
        /// <returns></returns>
        private Vector3 RotateDirection(Vector3 archrival)
        {
            var currentDirection = new Vector3(archrival.x - enemyTransform.position.x, 0,
                archrival.z - enemyTransform.position.z);
            return currentDirection;
        }

        /// <summary>
        ///     Метод расчета направления для стрельбы
        /// </summary>
        /// <param name="archrival"></param>
        /// <param name="rangeAccuracy"></param>
        /// <returns></returns>
        private Vector3 ShootDirection(Vector3 archrival, float rangeAccuracy)
        {
            if (hitChance == 0 && missChance == 0)
            {
                hitChance = rangeAccuracy;
                missChance = 10 - rangeAccuracy;
            }

            var isHit = Random.Range(-missChance, hitChance);
            Vector3 currentDirection;
            var shootDirection = new Vector3(archrival.x - enemyTransform.position.x,
                archrival.y - enemyTransform.position.y, archrival.z - enemyTransform.position.z);
            if (isHit >= 0 && isHit < hitChance)
            {
                var variant = Random.Range(-1, 1);
                currentDirection =
                    new Vector3(shootDirection.x + variant, shootDirection.y, shootDirection.z + variant);
                ;
                //Debug.LogWarning("ShotInPlayer");
                hitChance--;
            }
            else
            {
                var miss = Random.Range(-5, -3);
                var sideMiss = Random.Range(0, 2);
                if (sideMiss == 0)
                    miss = -miss;
                currentDirection = new Vector3(shootDirection.x + miss, shootDirection.y, shootDirection.z + miss);
                //Debug.LogWarning("MissShot");
                missChance--;
            }

            return currentDirection;
        }

        /// <summary>
        ///     Метод Стрельбы
        /// </summary>
        /// <param name="archrival"></param>
        private void RangeAttack(Vector3 archrival)
        {
            //Debug.Log("ShotCount: " + shotCount);
            //Debug.Log("HitCount: " + hitCount);
            timer = 0f;
            gunShotSound.Play();
            var currentDamage = rangeDamage;
            shootLine.useWorldSpace = true;
            shootLine.enabled = true;
            shootLine.SetPosition(0, gunBarrelEnd.position);
            shootLine.SetPosition(1, archrival + gunBarrelEnd.position);

            shotCount++;
            shootRay.origin = gunBarrelEnd.position;
            shootRay.direction = archrival;
            //Debug.Log(shootRay);

            if (Physics.Raycast(shootRay, out hit, 100f))
            {
                hitCount++;
                //Debug.LogError("Hit: " + hit.collider.name);
                SetDamage(hit.collider.GetComponent<IDamageable>(), currentDamage);
            }
        }

        private void SetDamage(IDamageable damageable, float currentDamage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Метод ближнего боя (в будущем)
        /// </summary>
        private void MeleeAttack()
        {
            timer = 0f;
        }

        /// <summary>
        ///     Метод отключения визуальных эффектов
        /// </summary>
        private void DisableEffects()
        {
            shootLine.enabled = false;
            shootLine.useWorldSpace = false;
        }

        /// <summary>
        ///     Метод нанесения урона цели
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="damage"></param>
        //private void SetDamage(IDamageable obj, float damage)
        //{
        //    if (obj != null) obj.TakeDamage(damage);
        //}

        /// <summary>
        ///     Активатор спец способности
        /// </summary>
        private void SpecialAbilityActivator()
        {
            if (meleeHitCount == 3) specialAbility = true;
        }

        /// <summary>
        ///     Метод выполнения специальной способности
        /// </summary>
        /// <param name="archrival"></param>
        private void SpecialAbility(Vector3 archrival)
        {
            //Debug.Log("Special");
            switchMode = true;
            currentAttackDistance = priorityDistance;
            gun.enabled = true;
            knife.enabled = false;
            var direction = RotateDirection(archrival);
            var delta = new Vector3(archrival.x - enemyTransform.position.x, archrival.y - enemyTransform.position.y,
                archrival.z - enemyTransform.position.z);
            var distancePointDirection = new Vector3(enemyTransform.position.x - delta.x * priorityDistance,
                enemyTransform.position.y - delta.y, enemyTransform.position.z - delta.z * priorityDistance);
            var distancePoint =
                new Vector3(distancePointDirection.x, distancePointDirection.y, distancePointDirection.z);
            move.Continue();
            move.Move(distancePoint, Speed.Throw);
            //enemyTransform.Translate(distancePoint);
            var distance = Mathf.Sqrt(Mathf.Pow(archrival.x - enemyTransform.position.x, 2) +
                                      Mathf.Pow(archrival.y - enemyTransform.position.y, 2) +
                                      Mathf.Pow(archrival.z - enemyTransform.position.z, 2));
            if (distance >= switchDistance)
            {
                specialAbility = false;
                meleeHitCount = 0;
                move.Stop();
                move.Rotate(RotateDirection(archrival));
                //Debug.LogError("FinishAbility");
            }
        }
    }
}