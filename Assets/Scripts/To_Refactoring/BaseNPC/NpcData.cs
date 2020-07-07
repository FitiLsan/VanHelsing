using UnityEngine;

namespace BeastHunter
{
    public abstract class NpcData : ScriptableObject
    {
        #region Fields

        public NpcStats BaseStats;

        #endregion


        #region Methods

        public virtual void Do(string how)
        {
            Debug.Log("I did something " + how);
        }

        public virtual void TakeDamage(NpcModel instance, Damage damage)
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
