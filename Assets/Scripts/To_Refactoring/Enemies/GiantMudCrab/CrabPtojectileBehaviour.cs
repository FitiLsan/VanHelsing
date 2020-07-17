using UnityEngine;


namespace BeastHunter
{
    public class CrabPtojectileBehaviour : MonoBehaviour, IDealDamage
    {
        #region Fields

        public Damage Damage;
        public BaseStatsClass Stats;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Destroy(gameObject, 5);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(TagManager.PLAYER) &&
                collision.transform.GetComponent<PlayerBehavior>() != null)
            {
                DealDamage(collision.transform.GetComponent<PlayerBehavior>(), Damage);
                Destroy(gameObject);
            }
        }

        #endregion


        #region Methods

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            PlayerBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();
            //BaseStatsClass enemyStats = _context.PlayerModel.GetStats().BaseStats;
            enemyBehavior.TakeDamageEvent(Services.SharedInstance.AttackService.CountDamage(damage, Stats, enemyBehavior.Stats));
            //enemyBehavior.TakeDamageEvent(Services.SharedInstance.AttackService.CountDamage(damage, Stats, enemyStats));
        }

        #endregion
    }
}

