using UnityEngine;
using UnityEngine.AI;
using UniRx;
using System.Collections.Generic;
using RootMotion.FinalIK;


namespace BeastHunter
{
    public sealed class BossModel : EnemyModel, IAwake, ITearDown
    {
        #region Properties

        public Transform LeftHand { get; }
        public Transform RightHand { get; }
        public Transform LeftFoot { get; }
        public Transform RightFoot { get; }
        public Transform BossTransform { get; }

        public Vector3 AnchorPosition { get; }

        public WeaponHitBoxBehavior LeftHandBehavior { get; set; }
        public WeaponHitBoxBehavior RightHandBehavior { get; set; }
        public WeaponHitBoxBehavior RightFingerTrigger { get; set; }
        public SphereCollider LeftHandCollider { get; set; }
        public SphereCollider RightHandCollider { get; set; }

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
        public BossData BossData { get; }
        public BossSettings BossSettings { get; }
        public BossStateMachine BossStateMachine { get; }

        public Animator BossAnimator { get; set; }
        public Collider Player { get; set; }

        public float CurrentSpeed { get; set; }
        public float AnimationSpeed { get; set; }

        public bool IsMoving { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsPlayerNear { get; set; }
        public bool IsPickUped { get; set; }

        public MovementPoint[] MovementPoints { get; set; }

        public List<GameObject> FoodList = new List<GameObject>();
        public GameObject Lair;
        public GameObject BossCurrentTarget;
        public Vector3 BossCurrentPosition;
        public GameObject SporePrefab;
        public GameObject Ruler;
        public GameObject StompPufPrefab;
        public GameObject HealAuraPrefab;
        public GameObject BarkBuffEffectPrefab;
        public GameObject CallOfForestEffectPrefab;

        public ParticleSystem leftStompEffect;
        public ParticleSystem rightStompEffect;
        public ParticleSystem healAura;
        public ParticleSystem barkBuffEffect;
        public ParticleSystem callOfForestEffect;

        public InteractionSystem InteractionSystem;
        public InteractionObject InteractionTarget;
        public InteractionObject CatchTarget;
        public GameObject targetParent;
        public bool interrupt;
        public FullBodyBipedEffector CurrentHand;
        public int ClosestTriggerIndex;
        public AimIK RightHandAimIK;
        public Transform RightHandAimIKTarget;
        public bool IsRage;
        public ParticleSystem Wisps;
        

        #endregion


        #region ClassLifeCycle

        public BossModel(GameObject objectOnScene, BossData data, LocationPosition position, GameContext context) : 
            base(objectOnScene, data)
        {
            Lair = GameObject.Find("Lair");

            BossData = data;
            BossSettings = BossData._bossSettings;
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

            BossRigitbody.isKinematic = BossData._bossSettings.IsRigitbodyKinematic;

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

            BossTransform.position = position.Position;
            BossTransform.eulerAngles = position.Eulers;
            BossTransform.localScale = position.Scale;

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

            if (objectOnScene.GetComponent<BossBehavior>() != null)
            {
                BossBehavior = objectOnScene.GetComponent<BossBehavior>();
            }
            else
            {
                BossBehavior = objectOnScene.AddComponent<BossBehavior>();
            }

            BossBehavior.SetType(InteractableObjectType.Enemy);
         //   BossBehavior.Stats = BossStats.MainStats;
            BossStateMachine = new BossStateMachine(context, this);

            Player = null;
            IsMoving = false;
            IsGrounded = false;
            IsPlayerNear = false;
            IsPickUped = false;

            CurrentSpeed = 0f;
            AnimationSpeed = BossData._bossSettings.AnimatorBaseSpeed;

            LeftHand = BossTransform.Find(BossSettings.LeftHandObjectPath);
            RightHand = BossTransform.Find(BossSettings.RightHandObjectPath);
            LeftFoot = BossTransform.Find(BossSettings.LeftFootObjectPath);
            RightFoot = BossTransform.Find(BossSettings.RightFootObjectPath);

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

            var leftFist = new GameObject("[LeftFist]");
            GameObject leftHandFist = GameObject.Instantiate(leftFist, LeftHand.position, LeftHand.rotation, LeftHand);
            GameObject.Destroy(leftFist);
            SphereCollider LeftHandTrigger = leftHandFist.AddComponent<SphereCollider>();
            LeftHandCollider = leftHandFist.AddComponent<SphereCollider>();
            LeftHandTrigger.radius = BossData._bossSettings.LeftHandHitBoxRadius;
            LeftHandTrigger.center = BossData._bossSettings.LeftHandHitBoxCenter;
            LeftHandTrigger.isTrigger = true;
            LeftHandCollider.radius = BossData._bossSettings.LeftHandHitBoxRadius - 0.2f;
            LeftHandCollider.center = BossData._bossSettings.LeftHandHitBoxCenter;
            LeftHandCollider.isTrigger = false;
            LeftHandCollider.enabled = false;
            LeftHand.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            LeftHandBehavior = leftHandFist.AddComponent<WeaponHitBoxBehavior>();
            LeftHandBehavior.SetType(InteractableObjectType.HitBox);
            LeftHandBehavior.IsInteractable = false;

            var rightFist = new GameObject("[RightFist]");
            GameObject rightHandFist = GameObject.Instantiate(rightFist, RightHand.position, RightHand.rotation, RightHand);
            GameObject.Destroy(rightFist);
            SphereCollider RightHandTrigger = rightHandFist.AddComponent<SphereCollider>();
            RightHandCollider = rightHandFist.AddComponent<SphereCollider>();
            RightHandTrigger.radius = BossData._bossSettings.RightHandHitBoxRadius;
            RightHandTrigger.center = BossData._bossSettings.RightHandHitBoxCenter;
            RightHandTrigger.isTrigger = true;
            RightHandCollider.radius = BossData._bossSettings.LeftHandHitBoxRadius - 0.2f;
            RightHandCollider.center = BossData._bossSettings.LeftHandHitBoxCenter;
            RightHandCollider.isTrigger = false;
            RightHandCollider.enabled = false;
            var rb = rightHandFist.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.mass = 30f;
            RightHandBehavior = rightHandFist.AddComponent<WeaponHitBoxBehavior>();
            RightHandBehavior.SetType(InteractableObjectType.HitBox);
            RightHandBehavior.IsInteractable = false;
            RightFingerTrigger = BossTransform.Find(BossSettings.RightFingerPath).GetComponent<WeaponHitBoxBehavior>();
            RightFingerTrigger.SetType(InteractableObjectType.HitBox);
            RightFingerTrigger.IsInteractable = false;

            StompPufPrefab = BossSettings.StompPuf;
            HealAuraPrefab = BossSettings.HealAura;
            BarkBuffEffectPrefab = BossSettings.BarkBuffEffect;
            CallOfForestEffectPrefab = BossSettings.CallOfForestEffect;

            GameObject _healAura = GameObject.Instantiate(HealAuraPrefab, BossTransform.position, Quaternion.identity, BossTransform);
            healAura = _healAura.GetComponent<ParticleSystem>();
            healAura.Stop();

            GameObject _barkBuffEffect = GameObject.Instantiate(BarkBuffEffectPrefab, BossTransform.position, BarkBuffEffectPrefab.transform.rotation, BossTransform);
            barkBuffEffect = _barkBuffEffect.GetComponent<ParticleSystem>();
            barkBuffEffect.Stop();

            GameObject _callOfForestEffect = GameObject.Instantiate(CallOfForestEffectPrefab, BossTransform.position + new Vector3(-0.65f, 5, 1), Quaternion.identity, BossTransform);
            callOfForestEffect = _callOfForestEffect.GetComponent<ParticleSystem>();
            callOfForestEffect.Stop();

            GameObject leftFootStompPuf = GameObject.Instantiate(StompPufPrefab, LeftFoot.position, LeftFoot.rotation, LeftFoot);
            leftStompEffect = leftFootStompPuf.GetComponent<ParticleSystem>();
            GameObject rightFootStompPuf = GameObject.Instantiate(StompPufPrefab, RightFoot.position, RightFoot.rotation, RightFoot);
            rightStompEffect = rightFootStompPuf.GetComponent<ParticleSystem>();

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
            SporePrefab = BossSettings.SporePrefab;
            Ruler = BossSettings.Ruler;
            GameObject.Instantiate(Ruler, BossTransform.position + Vector3.up, Quaternion.identity, BossTransform);

            InteractionSystem = BossTransform.GetComponent<InteractionSystem>();
            RightHandAimIK = BossTransform.GetComponent<AimIK>();
            RightHandAimIK.solver.IKPositionWeight = 0;
            RightHandAimIKTarget = new GameObject().transform;
            Wisps = BossTransform.Find("Wisps").GetComponent<ParticleSystem>();
            Wisps.maxParticles = 0;
        }

        #endregion


        #region Methods

        public void OnAwake()
        {
            BossStateMachine.OnAwake();
        }

        public override void TakeDamage(Damage damage)
        {
            if(CurrentStats.BaseStats.IsDead)
            {
                return;
            }

            CurrentStats.BaseStats.CurrentHealthPoints = CurrentStats.BaseStats.CurrentHealthPoints < damage.
                PhysicalDamage ? 0 : CurrentStats.BaseStats.CurrentHealthPoints - damage.PhysicalDamage;

            Debug.Log("Boss recieved: " + damage.PhysicalDamage + " of damage and has: " + 
                CurrentStats.BaseStats.CurrentHealthPoints + " of HP");
            
            if (damage.StunProbability > CurrentStats.DefenceStats.StunProbabilityResistance)
            {
                MessageBroker.Default.Publish(new OnBossStunnedEventClass());
            }
            else
            {
                MessageBroker.Default.Publish(new OnBossHittedEventClass());
            }

            DamageCheck(damage.PhysicalDamage);
            HealthCheck();
            BossStateMachine._mainState.DamageCounter(damage);
        }

        public void TearDown()
        {
            BossStateMachine.OnTearDown();
        }

        public void HealthCheck()
        {
            if (CurrentStats.BaseStats.CurrentHealthPoints <= 0)
            {
                BossStateMachine.SetCurrentStateAnyway(BossStatesEnum.Dead);
                return;
            }

            if (CurrentStats.BaseStats.CurrentHealthPart <= 0.5)
            {
                IsRage = true;
                return;
            }

            //if (CurrentStats.BaseStats.CurrentHealthPart <= 0.1f)
            //{
            //    var id = 3;
            //    BossStateMachine.BossSkills.ForceUseSkill(BossStateMachine.BossSkills.NonStateSkillDictionary, id);
            //    //  BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Retreating);
            //    return;
            //}

            if (CurrentStats.BaseStats.CurrentHealthPart <= 0.5f)
            {
                if (BossStateMachine.CurrentStateType != BossStatesEnum.Defencing)
                {
                    BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Defencing);
                    return;
                }
            }

            if (CurrentStats.BaseStats.CurrentHealthPart <= 0.2f)
            {
              //  BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Retreating);
                return;
            }
        }

        public void DamageCheck(float damage)
        {
            if (damage >= CurrentStats.BaseStats.MaximalHealthPoints * 0.2f)
            {
                Debug.Log("Hit 18% hp");
                if (BossStateMachine.CurrentStateType != BossStatesEnum.Defencing)
                {
                   // BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Defencing);
                }
            }
            else if (!BossStateMachine.CurrentState.IsBattleState)
            {
                BossStateMachine.SetCurrentStateOverride(BossStatesEnum.Attacking);
            }
        }

        #endregion
    }
}
