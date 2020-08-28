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
            if (collision.transform.CompareTag(TagManager.ENEMY) || collision.transform.CompareTag(TagManager.HITBOX))
            {
                if(collision.gameObject.GetComponent<InteractableObjectBehavior>().Type == InteractableObjectType.WeakHitBox)
                {
                    GlobalEventsModel.OnBossWeakPointHitted?.Invoke(collision.collider);
                }

                Context.NpcModels[GetParent(collision.transform).GetInstanceID()].TakeDamage(
                Services.SharedInstance.AttackService.CountDamage(
                    ProjectileDamage,
                        Context.NpcModels[GetParent(collision.transform).GetInstanceID()].GetStats().MainStats));
                Destroy(this.gameObject);
            }
        }

        private GameObject GetParent(Transform instance)
        {
            while (instance.parent != null)
            {
                instance = instance.parent;
            }

            return instance.gameObject;
        }

        #endregion
    }
}

