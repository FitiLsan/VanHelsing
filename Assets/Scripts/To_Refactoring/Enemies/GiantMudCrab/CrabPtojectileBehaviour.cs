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
            InteractableObjectBehavior enemyBehavior = enemy.GetComponent<PlayerBehavior>();
            enemyBehavior.OnTakeDamageHandler(Services.SharedInstance.AttackService.CountDamage(damage, Stats, enemyBehavior.Stats));
        }

        #endregion
    }
}

