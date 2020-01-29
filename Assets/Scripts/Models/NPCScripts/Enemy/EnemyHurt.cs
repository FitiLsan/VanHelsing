using UnityEngine;

namespace Models.NPCScripts.Enemy
{
    public class EnemyHurt
    {
        private readonly Color hardDamaged = new Color(0.9176471f, 0.2745098f, 0.2941176f, 1);
        private readonly MeshRenderer headMesh;
        private readonly Color lightDamaged = new Color(1, 0.7764706f, 0.6745283f, 1);
        private readonly Color mediumDamaged = new Color(1, 0.4705882f, 0.4823529f, 1);

        private Color noDamage = new Color(255, 231, 172, 255);
        private Color test1 = Color.yellow;
        private Color test2 = Color.blue;
        private Color test3 = Color.green;

        public EnemyHurt(MeshRenderer headMesh)
        {
            this.headMesh = headMesh;
        }

        public void Hurt(float lifePercent)
        {
            if (lifePercent <= 75 && lifePercent > 50)
                headMesh.material.color = lightDamaged;
            else if (lifePercent <= 50 && lifePercent > 25)
                headMesh.material.color = mediumDamaged;
            else if (lifePercent <= 25 && lifePercent > 0) headMesh.material.color = hardDamaged;
        }
    }
}