using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    public class EnemyChase
    {
        public delegate void ChaseContainer(string unityName);

        private readonly float chasingTime;
        private readonly Transform enemyTransform;
        private readonly EnemyMove move;
        private readonly float priorityDistance;

        private float runSpeed;
        private float timer;

        public EnemyChase(EnemyMove move, Transform enemyTransform, float chasingTime, float priorityDistance)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.chasingTime = chasingTime;
            this.priorityDistance = priorityDistance;
        }

        public static event ChaseContainer ChaseEvent;
        public static event ChaseContainer AttackSwitchEvent;

        /// <summary>
        ///     Метод погони
        ///     На вход получает центр зоны патрулирования, ее радиус и объект погони
        /// </summary>
        /// <param name="aim"></param>
        public void Chase(GameObject aim, float deltaTime)
        {
            timer += deltaTime;

            ///<summary>
            ///определяем дистанцию от центра зоны
            /// </summary>
            var distance = Mathf.Sqrt(Mathf.Pow(aim.transform.position.x - enemyTransform.position.x, 2) +
                                      Mathf.Pow(aim.transform.position.y - enemyTransform.position.y, 2) +
                                      Mathf.Pow(aim.transform.position.z - enemyTransform.position.z, 2));
            move.Move(aim.transform.position, Speed.Run);
            if (timer > chasingTime)
            {
                StopChase();
                timer = 0f;
            }

            if (distance < priorityDistance) AttackSwitchEvent(enemyTransform.name);
        }

        /// <summary>
        ///     Метод прекращения погони
        /// </summary>
        public void StopChase()
        {
            ChaseEvent(enemyTransform.name);
        }
    }
}