using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    public class EnemyComingHome
    {
        public delegate void ComingHomeContainer(string unitName);

        private readonly Transform enemyTransform;
        private readonly Vector3 homePoint;

        private readonly EnemyMove move;

        public EnemyComingHome(EnemyMove move, Transform enemyTransform, Vector3 homePoint)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.homePoint = homePoint;
        }

        public static event ComingHomeContainer ComingHomeEvent;

        public void ComingHome()
        {
            var distance = Mathf.Sqrt(Mathf.Pow(homePoint.x - enemyTransform.position.x, 2) +
                                      Mathf.Pow(homePoint.y - enemyTransform.position.y, 2) +
                                      Mathf.Pow(homePoint.z - enemyTransform.position.z, 2));
            move.Move(homePoint);
            if (distance < 2f) ComingHomeEvent(enemyTransform.name);
        }
    }
}