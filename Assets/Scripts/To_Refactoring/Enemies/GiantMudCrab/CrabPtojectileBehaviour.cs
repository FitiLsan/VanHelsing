using UnityEngine;


namespace BeastHunter
{
    public class CrabPtojectileBehaviour : MonoBehaviour, IDealDamage
    {
        #region Fields

        public DamageStruct Damage;

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

        public void DealDamage(InteractableObjectBehavior enemy, DamageStruct damage)
        {
            enemy.GetComponent<PlayerBehavior>().OnTakeDamageHandler(damage);
            Debug.Log("Dealed " + damage.damage + " damage to player!");
        }

        #endregion
    }
}

