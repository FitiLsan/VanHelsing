using UnityEngine;
using UniRx;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewCrossbowBoltData", menuName = "CreateProjectileData/CreateCrossbowBoltData", order = 0)]
    public sealed class CrossbowBoltData : ProjectileData
    {
        #region Methods

        public override bool FilterCollision(Collision touchedCollider)
        {
            return touchedCollider.transform.GetMainParent().GetComponentInChildren<InteractableObjectBehavior>()?.
                Type != InteractableObjectType.Player;
        }

        public override void HitProjectile(IProjectile projectileInterface, Collision touchedCollider)
        {
            bool isHittedEnemy = touchedCollider.transform.GetMainParent().gameObject.
                TryGetComponent(out InteractableObjectBehavior touchedBehavior);

            if (isHittedEnemy)
            {
                switch (touchedBehavior.Type)
                {
                    case InteractableObjectType.Enemy:
                        break;
                    case InteractableObjectType.WeakHitBox:
                        MessageBroker.Default.Publish(
                            new OnBossWeakPointHittedEventClass { WeakPointCollider = touchedCollider.collider });
                        break;
                    default:
                        break;
                }

                Context.NpcModels[touchedCollider.transform.GetMainParent().gameObject.GetInstanceID()].TakeDamage(Services.
                    SharedInstance.AttackService.CountDamage(ProjectileDamage, Context.NpcModels[touchedCollider.
                        transform.GetMainParent().gameObject.GetInstanceID()].GetStats().MainStats));
                StackInObject(projectileInterface, touchedCollider, true);
            }
            else
            {
                StackInObject(projectileInterface, touchedCollider, false);
            }
        }

        private void StackInObject(IProjectile projectileInterface, Collision touchedCollider, bool doChangeParent)
        {
            if (doChangeParent)
            {
                projectileInterface.GameObject.transform.GetMainParent().parent = touchedCollider.transform;
            }

            Destroy(projectileInterface.GameObject.GetComponent<Rigidbody>());
            Destroy(projectileInterface.GameObject.GetComponent<SphereCollider>());
            Destroy(projectileInterface.GameObject.GetComponent<ProjectileBehavior>());
        }

        #endregion
    }
}

