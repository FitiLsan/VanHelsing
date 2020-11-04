using UnityEngine;
using UnityEngine.AI;
using UniRx;


namespace BeastHunter
{
    public sealed class BossModel : EnemyModel
    {
        #region Properties

        public Transform LeftHand { get; }
        public Transform RightHand { get; }
        public Transform BossTransform { get; }

        public Vector3 AnchorPosition { get; }

        public WeaponHitBoxBehavior LeftWeaponBehavior { get; set; }
        public WeaponHitBoxBehavior RightWeaponBehavior { get; set; }
        public WeaponItem LeftHandWeapon { get; set; }
        public WeaponItem RightHandWeapon { get; set; }

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
        public BossData BossData { get; }
        public BossSettings BossSettings { get; }
        public EnemyStats BossStats { get; }
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

        public BossModel(GameObject prefab, BossData bossData, Vector3 groundPosition, GameContext context)
        {
            BossData = bossData;
            BossSettings = BossData.BossSettings;
            BossStats = BossData.BaseStats;
            BossTransform = prefab.transform;
            BossTransform.rotation = Quaternion.Euler(0, BossSettings.InstantiateDirection, 0);
            BossTransform.name = BossSettings.InstanceName;
            BossTransform.tag = BossSettings.InstanceTag;
            BossTransform.gameObject.layer = BossSettings.InstanceLayer;

            Transform[] children = BossTransform.GetComponentsInChildren<Transform>();

            foreach (var child in children)
            {
                child.gameObject.layer = BossSettings.InstanceLayer;
            }

            if (prefab.GetComponent<Rigidbody>() != null)
            {
                BossRigitbody = prefab.GetComponent<Rigidbody>();
            }
            else
            {
                BossRigitbody = prefab.AddComponent<Rigidbody>();
                BossRigitbody.freezeRotation = true;
                BossRigitbody.mass = BossSettings.RigitbodyMass;
                BossRigitbody.drag = BossSettings.RigitbodyDrag;
                BossRigitbody.angularDrag = BossSettings.RigitbodyAngularDrag;
            }

            BossRigitbody.isKinematic = BossData.BossSettings.IsRigitbodyKinematic;

            if (prefab.GetComponent<CapsuleCollider>() != null)
            {
                BossCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
            }
            else
            {
                BossCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
                BossCapsuleCollider.center = BossSettings.CapsuleColliderCenter;
                BossCapsuleCollider.radius = BossSettings.CapsuleColliderRadius;
                BossCapsuleCollider.height = BossSettings.CapsuleColliderHeight;
            }

            BossCapsuleCollider.transform.position = groundPosition;

            if (prefab.GetComponent<SphereCollider>() != null)
            {
                BossSphereCollider = prefab.GetComponent<SphereCollider>();
                BossSphereCollider.isTrigger = true;
            }
            else
            {
                BossSphereCollider = prefab.AddComponent<SphereCollider>();
                BossSphereCollider.center = BossSettings.SphereColliderCenter;
                BossSphereCollider.radius = BossSettings.SphereColliderRadius;
                BossSphereCollider.isTrigger = true;
            }

            if (prefab.GetComponent<Animator>() != null)
            {
                BossAnimator = prefab.GetComponent<Animator>();
            }
            else
            {
                BossAnimator = prefab.AddComponent<Animator>();
            }

            BossAnimator.runtimeAnimatorController = BossSettings.BossAnimator;
            BossAnimator.applyRootMotion = false;

            if (prefab.GetComponent<BossBehavior>() != null)
            {
                BossBehavior = prefab.GetComponent<BossBehavior>();
            }
            else
            {
                BossBehavior = prefab.AddComponent<BossBehavior>();
            }

            GameObject movement = GameObject.Instantiate(BossData._movementPrefab);
            MovementPath movementPath = movement.GetComponent<MovementPath>();

            if (!movementPath)
            {
                movementPath = movement.AddComponent<MovementPath>();
            }
            
            MovementPoints = movementPath.GetPoints().ToArray();
            MovementLoop = movementPath.Loop;

            BossBehavior.SetType(InteractableObjectType.Enemy);
            BossBehavior.Stats = BossStats.MainStats;
            BossStateMachine = new BossStateMachine(context, this);

            Player = null;
            IsMoving = false;
            IsGrounded = false;
            IsPlayerNear = false;

            CurrentSpeed = 0f;
            AnimationSpeed = BossData.BossSettings.AnimatorBaseSpeed;

            LeftHand = BossAnimator.GetBoneTransform(HumanBodyBones.LeftHand);
            RightHand = BossAnimator.GetBoneTransform(HumanBodyBones.RightHand);

            AnchorPosition = BossTransform.position;

            if (prefab.GetComponent<NavMeshAgent>() != null)
            {
                BossNavAgent = prefab.GetComponent<NavMeshAgent>();
            }
            else
            {
                BossNavAgent = prefab.AddComponent<NavMeshAgent>();
            }

            LeftHandWeapon = Data.BossFeast;
            GameObject leftHandWeapon = GameObject.Instantiate(LeftHandWeapon.WeaponPrefab, LeftHand);
            SphereCollider LeftHandTrigger = leftHandWeapon.GetComponent<SphereCollider>();
            LeftHandTrigger.radius = BossData.BossSettings.LeftHandHitBoxRadius;
            LeftHandTrigger.center = BossData.BossSettings.LeftHandHitBoxCenter;
            LeftHandTrigger.isTrigger = true;
            LeftHand.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            LeftWeaponBehavior = leftHandWeapon.GetComponent<WeaponHitBoxBehavior>();
            LeftWeaponBehavior.SetType(InteractableObjectType.HitBox);
            LeftWeaponBehavior.IsInteractable = false;

            RightHandWeapon = Data.BossFeast;
            GameObject rightHandWeapon = GameObject.Instantiate(RightHandWeapon.WeaponPrefab, RightHand);
            SphereCollider RightHandTrigger = rightHandWeapon.GetComponent<SphereCollider>();
            RightHandTrigger.radius = BossData.BossSettings.RightHandHitBoxRadius;
            RightHandTrigger.center = BossData.BossSettings.RightHandHitBoxCenter;
            RightHandTrigger.isTrigger = true;
            RightHand.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            RightWeaponBehavior = rightHandWeapon.GetComponent<WeaponHitBoxBehavior>();
            RightWeaponBehavior.SetType(InteractableObjectType.HitBox);
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
            CurrentHealth = BossStats.MainStats.HealthPoints;
        }

        #endregion


        #region Methods

        public override void DoSmth(string how)
        {

        }

        public override void OnAwake()
        {
            BossStateMachine.OnAwake();
        }

        public override void Execute()
        {
            BossStateMachine.Execute();
        }

        public override EnemyStats GetStats()
        {
            return BossData.BaseStats;
        }

        public override void TakeDamage(Damage damage)
        {
            CurrentHealth = CurrentHealth < damage.PhysicalDamage ? 0 : CurrentHealth - damage.PhysicalDamage;

            Debug.Log("Boss has: " + CurrentHealth + " of HP");

            if (damage.StunProbability > BossData.BaseStats.MainStats.StunResistance)
            {
                GlobalEventsModel.OnBossStunned?.Invoke();
            }
        }

        public override void OnTearDown()
        {
            BossStateMachine.OnTearDown();
        }

        #endregion
    }
}
