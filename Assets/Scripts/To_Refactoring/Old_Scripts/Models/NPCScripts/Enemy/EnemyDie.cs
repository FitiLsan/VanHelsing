using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    public class EnemyDie
    {
        public delegate void DieContainer(string unitName);

        private bool animStarted;
        private readonly float dyingTime = 0.5f;
        private readonly Transform enemyTransform;
        private float frameTimer;
        private readonly float timeBetweenFrames = 0.05f;

        private float timer;

        public EnemyDie(Transform local)
        {
            enemyTransform = local;
        }

        public event DieContainer DieEvent;

        /// <summary>
        ///     Метод демонстрации смерти врага
        /// </summary>
        /// <param name="mesh"></param>
        public void Die(MeshRenderer mesh, float deltaTime)
        {
            if (!animStarted)
            {
                animStarted = true;
                timer = 0f;
                mesh.material.color = Color.red;
            }
            else if (animStarted && timer < dyingTime)
            {
                if (frameTimer < timeBetweenFrames)
                {
                    frameTimer += deltaTime;
                }
                else
                {
                    frameTimer = 0f;
                    timer += deltaTime;
                    enemyTransform.localScale += new Vector3(0.2f, -0.1f, 0.2f);
                }
            }
            else
            {
                //Debug.Log("invis");
                DieEvent(enemyTransform.name);
                animStarted = false;
            }
            //if (invisibleSwitch < 255)
            //{
            //    mesh.material.color = new Color(255, invisibleSwitch, invisibleSwitch, 255);

            //}
        }
    }
}