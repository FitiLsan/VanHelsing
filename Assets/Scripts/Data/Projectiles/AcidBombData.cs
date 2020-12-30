using UnityEngine;
using UniRx;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewAcidBombData", menuName = "CreateProjectileData/CreateActidBombData", order = 0)]
    public sealed class AcidBombData : ProjectileData
    {
        #region Methods

        public override bool FilterCollision(Collision touchedCollider)
        {
            bool isProjectileTouchedEnemy = false;

            InteractableObjectBehavior touchedBehavior = touchedCollider.gameObject.GetComponent<InteractableObjectBehavior>();

            if(touchedBehavior != null)
            {
                isProjectileTouchedEnemy = touchedBehavior.Type == InteractableObjectType.Enemy ||
                touchedBehavior.Type == InteractableObjectType.WeakHitBox;
            }

            return isProjectileTouchedEnemy;
        }

        public override void HitProjectile(IProjectile projectileInterface, Collision touchedCollider)
        {
            InteractableObjectBehavior touchedBehavior = touchedCollider.gameObject.
                GetComponent<InteractableObjectBehavior>();

            if (touchedBehavior.Type == InteractableObjectType.WeakHitBox)
            {
                MessageBroker.Default.Publish(
                    new OnBossWeakPointHittedEventClass { WeakPointCollider = touchedCollider.collider });
            }

            Context.NpcModels[touchedCollider.transform.GetMainParent().gameObject.GetInstanceID()].TakeDamage(Services.
                SharedInstance.AttackService.CountDamage(ProjectileDamage, Context.NpcModels[touchedCollider.
                    transform.GetMainParent().gameObject.GetInstanceID()].GetStats().MainStats));

            DestroyProjectile(projectileInterface);
        }

        #endregion
    }
}

