using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    /// <summary>
    ///     Класс контроллирующий патрулирование нпс
    /// </summary>
    public class EnemyPatrolController
    {
        public delegate void PatrolWaiter(string unitName);

        private int count;
        private Vector3 currentDirection;

        private Vector3 currentPoint;
        private readonly Transform enemyTransform;

        private readonly EnemyMove move;

        public EnemyPatrolController(EnemyMove move, Transform local)
        {
            this.move = move;
            enemyTransform = local;
        }

        public static event PatrolWaiter PatrolEvent;

        /// <summary>
        ///     Вызываемый извне метод для патрулирования по заданному маршруту
        /// </summary>
        /// <param name="route"></param>
        public void Patrol(Vector3[] route)
        {
            currentPoint = route[count];
            if (Distance() && count < route.Length - 1)
            {
                Debug.Log("Count: " + count);
                count++;
            }
            else if (Distance() && count == route.Length - 1)
            {
                PatrolEvent(enemyTransform.name);
                count = 0;
            }
            else if (!Distance())
            {
                move.Move(currentPoint);
            }
        }

        public void Stop()
        {
            move.Stop();
        }

        /// <summary>
        ///     Проверка расстояния нпс до точки перемещения
        /// </summary>
        /// <returns></returns>
        private bool Distance()
        {
            var dist = Mathf.Sqrt(Mathf.Pow(currentPoint.x - enemyTransform.position.x, 2) +
                                  Mathf.Pow(currentPoint.y - enemyTransform.position.y, 2) +
                                  Mathf.Pow(currentPoint.z - enemyTransform.position.z, 2));
            if (dist > 3)
                return false;
            return true;
        }
    }
}