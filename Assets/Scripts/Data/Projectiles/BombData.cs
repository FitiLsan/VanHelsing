using UnityEngine;
using UniRx;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewBombtData", menuName = "Character/CreateProjectileData/CreateBombData", order = 0)]
    public sealed class BombData : ProjectileData
    {
        #region Fields

        [SerializeField] private float _explosionHearingDistance;

        #endregion


        #region Properties

        public float ExplosionHearingDistance => _explosionHearingDistance;

        #endregion


        #region Methods

        public override bool FilterCollision(Collision touchedCollider)
        {
            InteractableObjectBehavior touched = touchedCollider.collider.transform.root.
                GetComponentInChildren<InteractableObjectBehavior>();
            return touched == null || touched.Type != InteractableObjectType.Player;
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
            }

            ExplodeBomb(projectileInterface, touchedCollider);
        }

        private void ExplodeBomb(IProjectile projectileInterface, Collision touchedCollider)
        {
            Services.SharedInstance.AnnouncementService.MakeNoise(new Noise(projectileInterface.GameObject.transform.position, 
                NoiseType.Explosion, ExplosionHearingDistance));
            Services.SharedInstance.AnnouncementService.MakeSmell(new Smell(projectileInterface.GameObject.transform, LureSmellTypeEnum.meaty, 1000f ));
            Rigidbody bombRigidbody = projectileInterface.GameObject.GetComponent<Rigidbody>();
            bombRigidbody.velocity = Vector3.zero;
            bombRigidbody.isKinematic = true;
            projectileInterface.GameObject.GetComponent<ParticleSystem>().Play();
            Destroy(projectileInterface.GameObject.GetComponent<ProjectileBehavior>());
            Destroy(projectileInterface.GameObject.GetComponent<MeshRenderer>());
            Destroy(projectileInterface.GameObject.GetComponent<Collider>());
            AudioSource projectileAudioSource = projectileInterface.GameObject.GetComponent<AudioSource>();
            projectileAudioSource.PlayOneShot(CollisionSound);
            Destroy(projectileAudioSource, CollisionSound.SoundClip.length);
            Destroy(projectileInterface.GameObject, CollisionSound.SoundClip.length);
        }

        #endregion
    }
}

