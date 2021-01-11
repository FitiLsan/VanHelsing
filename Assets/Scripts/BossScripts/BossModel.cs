using UnityEngine;
using UnityEngine.AI;
using UniRx;


namespace BeastHunter
{
    public sealed class BossModel : EnemyModel, IAwake, ITearDown
    {
        #region Properties

        public Transform LeftHand { get; }
        public Transform RightHand { get; }
        public Transform BossTransform { get; }

        public Vector3 AnchorPosition { get; }

        public WeaponHitBoxBehavior LeftWeaponBehavior { get; set; }
        public WeaponHitBoxBehavior RightWeaponBehavior { get; set; }
        public WeaponData WeaponData { get; set; }

        public WeakPointData FirstWeakPointData { get; set; }
        public WeakPointData SecondWeakPointData { get; set; }
        public WeakPointData ThirdWeakPointData { get; set; }
        public HitBoxBehavior FirstWeakPointBehavior { get; set; }
        public HitBoxBehavior SecondWeakPointBehavior { get; set; }
        public HitBoxBehavior ThirdWeakPointBehavior { get; set; }

        public CapsuleCollider BossCapsuleCollider { get; }
        public SphereCollider BossSphereCollider { get; }
        public Rigidbody BossRigitbody { get; }
        public NavMeshAgent BossNavAgent { get; }
        public BossBehavior BossBehavior { get; }
        public BossData ThisBossData { get; }
        public BossSettings BossSettings { get; }
        public BossStateMachine BossStateMachine { get; }

        public Animator BossAnimator { get; set; }
        public Collider Player { get; set; }

        public float CurrentSpeed { get; set; }
        public float AnimationSpeed { get; set; }

        public bool IsMoving { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsPlayerNear { get; set; }

        public MovementPoint[] MovementPoints { get; set; }
        
        public bool MovementLoop { get; set; }

        #endregion


        #region ClassLifeCycle

        public BossModel(GameObject objectOnScene, EnemyData bossData, Vector3 groundPosition, GameContext context) : 
            base(objectOnScene, bossData)
        {
            ThisBossData = bossData as BossData;
            BossSettings = ThisBossData.BossSettings;
            BossTransform = objectOnScene.transform;
            BossTransform.rotation = Quaternion.Euler(0, BossSettings.InstantiateDirection, 0);
            BossTransform.name = BossSettings.InstanceName;
            BossTransform.tag = BossSettings.InstanceTag;
            BossTransform.gameObject.layer = BossSettings.InstanceLayer;

            Transform[] children = BossTransform.GetComponentsInChildren<Transform>();

            foreach (var child in children)
            {
                child.gameObject.layer = BossSettings.InstanceLayer;
            }

            if (objectOnScene.GetComponent<Rigidbody>() != null)
            {
                BossRigitbody = objectOnScene.GetComponent<Rigidbody>();
            }
            else
            {
                BossRigitbody = objectOnScene.AddComponent<Rigidbody>();
                BossRigitbody.freezeRotation = true;
                BossRigitbody.mass = BossSettings.RigitbodyMass;
                BossRigitbody.drag = BossSettings.RigitbodyDrag;
                BossRigitbody.angularDrag = BossSettings.RigitbodyAngularDrag;
            }

            BossRigitbody.isKinematic = ThisBossData.BossSettings.IsRigitbodyKinematic;

            if (objectOnScene.GetComponent<CapsuleCollider>() != null)
            {
                BossCapsuleCollider = objectOnScene.GetComponent<CapsuleCollider>();
            }
            else
            {
                BossCapsuleCollider = objectOnScene.AddComponent<CapsuleCollider>();
                BossCapsuleCollider.center = BossSettings.CapsuleColliderCenter;
                BossCapsuleCollider.radius = BossSettings.CapsuleColliderRadius;
                BossCapsuleCollider.height = BossSettings.CapsuleColliderHeight;
            }

            BossTransform.position = groundPosition;

            if (objectOnScene.GetComponent<SphereCollider>() != null)
            {
                BossSphereCollider = objectOnScene.GetComponent<SphereCollider>();
                BossSphereCollider.isTrigger = true;
            }
            else
            {
                BossSphereCollider = objectOnScene.AddComponent<SphereCollider>();
                BossSphereCollider.center = BossSettings.SphereColliderCenter;
                BossSphereCollider.radius = BossSettings.SphereColliderRadius;
                BossSphereCollider.isTrigger = true;
            }

            if (objectOnScene.GetComponent<Animator>() != null)
            {
                BossAnimator = objectOnScene.GetComponent<Animator>();
            }
            else
            {
                BossAnimator = objectOnScene.AddComponent<Animator>();
            }

            BossAnimator.runtimeAnimatorController = BossSettings.BossAnimator;
            BossAnimator.applyRootMotion = false;

            if (objectOnScene. GetComponent<BossBehavior>() != null)
            {
                BossBehavior = objectOnScene.GetComponent<BossBehavior>();
            }
            else
            {
                throw new System.Exception("Boss has no behavior script");
            }

            GameObject movement = GameObject.Instantiate(ThisBossData.MovementPrefab);
            MovementPath movementPath = movement.GetComponent<MovementPath>();

            if (!movementPath)
            {
                movementPath = movement.AddComponent<MovementPath>();
            }
            
            MovementPoints = movementPath.GetPoints().ToArray();
            MovementLoop = movementPath.Loop;

            BossStateMachine = new BossStateMachine(context, this);

            Player = null;
            IsMoving = false;
            IsGrounded = false;
            IsPlayerNear = false;

            CurrentSpeed = 0f;
            AnimationSpeed = ThisBossData.BossSettings.AnimatorBaseSpeed;

            LeftHand = BossTransform.Find(BossSettings.LeftHandObjectPath);
            RightHand = BossTransform.Find(BossSettings.RightHandObjectPath);

            AnchorPosition = BossTransform.position;

            if (objectOnScene.GetComponent<NavMeshAgent>() != null)
            {
                BossNavAgent = objectOnScene.GetComponent<NavMeshAgent>();
            }
            else
            {
                BossNavAgent = objectOnScene.AddComponent<NavMeshAgent>();
            }

            WeaponData = Data.BossFeasts;

            GameObject leftHandWeapon = GameObject.Instantiate((WeaponData as TwoHandedWeaponData).
                LeftActualWeapon.WeaponPrefab, LeftHand);
            SphereCollider LeftHandTrigger = leftHandWeapon.GetComponent<SphereCollider>();
            LeftHandTrigger.radius = ThisBossData.BossSettings.LeftHandHitBoxRadius;
            LeftHandTrigger.center = ThisBossData.BossSettings.LeftHandHitBoxCenter;
            LeftHandTrigger.isTrigger = true;
            LeftHand.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            LeftWeaponBehavior = leftHandWeapon.GetComponent<WeaponHitBoxBehavior>();
            LeftWeaponBehavior.IsInteractable = false;

            GameObject rightHandWeapon = GameObject.Instantiate((WeaponData as TwoHandedWeaponData).
                RightActualWeapon.WeaponPrefab, RightHand);
            SphereCollider RightHandTrigger = rightHandWeapon.GetComponent<SphereCollider>();
            RightHandTrigger.radius = ThisBossData.BossSettings.RightHandHitBoxRadius;
            RightHandTrigger.center = ThisBossData.BossSettings.RightHandHitBoxCenter;
            RightHandTrigger.isTrigger = true;
            RightHand.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            RightWeaponBehavior = rightHandWeapon.GetComponent<WeaponHitBoxBehavior>();
            RightWeaponBehavior.IsInteractable = false;

            FirstWeakPointData = Data.BossFirstWeakPoint;
            GameObject firstWeakPoint = GameObject.Instantiate(FirstWeakPointData.InstancePrefab,
                BossAnimator.GetBoneTransform(HumanBodyBones.Chest));
            firstWeakPoint.tag = TagManager.HITBOX;
            firstWeakPoint.transform.localPosition = FirstWeakPointData.PrefabLocalPosition;
            FirstWeakPointBehavior = firstWeakPoint.GetComponent<HitBoxBehavior>();
            FirstWeakPointBehavior.AdditionalDamage = FirstWeakPointData.AdditionalDamage;

            SecondWeakPointData = Data.BossSecondWeakPoint;
            GameObject secondWeakPoint = GameObject.Instantiate(SecondWeakPointData.InstancePrefab,
                BossAnimator.GetBoneTransform(HumanBodyBones.Hips));
            secondWeakPoint.tag = TagManager.HITBOX;
            secondWeakPoint.transform.localPosition = SecondWeakPointData.PrefabLocalPosition;
            SecondWeakPointBehavior = secondWeakPoint.GetComponent<HitBoxBehavior>();
            SecondWeakPointBehavior.AdditionalDamage = SecondWeakPointData.AdditionalDamage;

            ThirdWeakPointData = Data.BossThirdWeakPoint;
            GameObject thirdWeakPoint = GameObject.Instantiate(ThirdWeakPointData.InstancePrefab,
                BossAnimator.GetBoneTransform(HumanBodyBones.RightLowerLeg));
            thirdWeakPoint.tag = TagManager.HITBOX;
            thirdWeakPoint.transform.localPosition = ThirdWeakPointData.PrefabLocalPosition;
            ThirdWeakPointBehavior = thirdWeakPoint.GetComponent<HitBoxBehavior>();
            ThirdWeakPointBehavior.AdditionalDamage = ThirdWeakPointData.AdditionalDamage;

            BossNavAgent.acceleration = BossSettings.NavMeshAcceleration;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            BossStateMachine.OnAwake();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            BossStateMachine.OnTearDown();
        }

        #endregion


        #region Methods

        public override void TakeDamage(Damage damage)
        {
            CurrentStats.BaseStats.CurrentHealthPoints -= damage.GetTotalDamage();

            Debug.Log("Boss recieved: " + damage.GetTotalDamage() + " of damage and has: " + 
                CurrentStats.BaseStats.CurrentHealthPoints + " of HP");

            if (damage.StunProbability > CurrentStats.DefenceStats.StunProbabilityResistance)
            {
                MessageBroker.Default.Publish(new OnBossStunnedEventClass());
            }
            else
            {
                MessageBroker.Default.Publish(new OnBossHittedEventClass());
            }
        }

        #endregion
    }
}
