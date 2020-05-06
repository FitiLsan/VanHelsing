using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    public class EnemyIdleController
    {
        public delegate void IdleWaiter(string unitName);

        public static IdleWaiter IdleEvent;
        private Animation anim;
        private bool animStarted;

        private readonly Transform enemyTransform;
        private float idleTime;
        private float timer;

        public EnemyIdleController(Transform enemyTransform)
        {
            this.enemyTransform = enemyTransform;
        }

        public void Idle()
        {
            if (!animStarted)
            {
                idleTime = Random.Range(3, 10);
                timer = 0;
                animStarted = true;
            }
            else if (animStarted && timer < idleTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                IdleEvent(enemyTransform.name);
                animStarted = false;
            }
        }
    }
}