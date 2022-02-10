using UnityEngine;
using UniRx;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewBallistaBoltData", menuName = "Character/CreateProjectileData/CreateBallistaBoltData", order = 0)]
    public sealed class BallistaBoltData : ProjectileData
    {
        #region Methods

        public override bool FilterCollision(Collision touchedCollider)
        {
            return touchedCollider.collider.GetComponent<InteractableObjectBehavior>()?.
                Type != InteractableObjectType.Player;
        }

        public override void HitProjectile(IProjectile projectileInterface, Collision touchedCollider)
        {
            bool isHittedEnemy = touchedCollider.transform.gameObject.TryGetComponent(out InteractableObjectBehavior touchedBehavior);

            if (isHittedEnemy)
            {
                switch (touchedBehavior.Type)
                {
                    case InteractableObjectType.Enemy:
                        Services.SharedInstance.AttackService.CountAndDealDamage(ProjectileDamage,
                            touchedCollider.transform.root.gameObject.GetInstanceID());
                        break;
                    case InteractableObjectType.WeakHitBox:
                        MessageBroker.Default.Publish(
                            new OnBossWeakPointHittedEventClass { WeakPointCollider = touchedCollider.collider });
                        break;
                    default:
                        break;
                }

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
                projectileInterface.GameObject.transform.root.parent = touchedCollider.transform;
            }
            var behavior = projectileInterface.GameObject.GetComponent<InteractableObjectBehavior>();
            if (behavior != null && behavior.Type.Equals(InteractableObjectType.Fire))
            {
                Services.SharedInstance.BuffService.AddTemporaryBuff(touchedCollider.gameObject.GetInstanceID(), Resources.Load("Data/Buffs/FireDebuffData") as TemporaryBuff);
            }

            //  Destroy(projectileInterface.GameObject.GetComponent<Rigidbody>());
            //  Destroy(projectileInterface.GameObject.GetComponent<SphereCollider>());
             // Destroy(projectileInterface.GameObject.GetComponent<ProjectileBehavior>());

            AudioSource projectileAudioSource = projectileInterface.GameObject.GetComponent<AudioSource>();
            projectileAudioSource.PlayOneShot(CollisionSound);
           //  Destroy(projectileAudioSource, CollisionSound.SoundClip.length);
        }

        #endregion
    }
}

