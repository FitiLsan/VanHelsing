using UnityEngine;

namespace BeastHunter
{
    public abstract class EnemyData : ScriptableObject
    {
        #region Fields

        public EnemyStats BaseStats;

        #endregion


        #region Methods

        public virtual void Do(string how)
        {
            Debug.Log("I did something " + how);
        }

        public virtual void TakeDamage(EnemyModel instance, Damage damage)
        {
            instance.CurrentHealth -= damage.PhysicalDamage;
            if (instance.CurrentHealth <= 0)
            {
                instance.IsDead = true;
                //instance.GetComponent<InteractableObjectBehavior>().enabled = false;
            }
        }

        #endregion
    }

}
