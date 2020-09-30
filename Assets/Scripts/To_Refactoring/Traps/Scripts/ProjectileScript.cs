using UnityEngine;


namespace BeastHunter
{
    public sealed class ProjectileScript: MonoBehaviour
    {
        #region Fields

        public Damage ProjectileDamage;
        [HideInInspector] public GameContext Context;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Destroy(this.gameObject, 10f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(TagManager.ENEMY))
            {
                Context.EnemyModels[collision.gameObject.GetInstanceID()].TakeDamage(
                Services.SharedInstance.AttackService.CountDamage(
                ProjectileDamage,
                Context.EnemyModels[collision.gameObject.GetInstanceID()].GetStats().
                MainStats));
                Destroy(this.gameObject);
            }
        }

        #endregion
    }
}

