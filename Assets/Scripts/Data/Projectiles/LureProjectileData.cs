using UnityEngine;
using UniRx;
using Extensions;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewLureData", menuName = "Character/CreateProjectileData/CreateLureData", order = 0)]
    public sealed class LureProjectileData : ProjectileData
    {
        #region Fields

        [SerializeField] private float _smellingDistance;
        [SerializeField] private LureSmellTypeEnum _lureSmellType;

        #endregion


        #region Properties

        public float SmellingDistance => _smellingDistance;
        public LureSmellTypeEnum LureSmellType => _lureSmellType; 

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
            StartSmelling(projectileInterface, touchedCollider);
        }

        private void StartSmelling(IProjectile projectileInterface, Collision touchedCollider)
        {
            Services.SharedInstance.AnnouncementService.MakeSmell(new Smell(projectileInterface.GameObject.transform, _lureSmellType, _smellingDistance ));
            Rigidbody bombRigidbody = projectileInterface.GameObject.GetComponent<Rigidbody>();
            bombRigidbody.velocity = Vector3.zero;
            bombRigidbody.isKinematic = true;
            projectileInterface.GameObject.GetComponent<ParticleSystem>().Play();
            AudioSource projectileAudioSource = projectileInterface.GameObject.GetComponent<AudioSource>();
            projectileAudioSource.PlayOneShot(CollisionSound);
        }

        #endregion
    }
}

