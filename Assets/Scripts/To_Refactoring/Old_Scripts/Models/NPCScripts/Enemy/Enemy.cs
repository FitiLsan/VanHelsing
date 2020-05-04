using Interfaces;
using Models.ConditionsAndActions;
using Models.ConditionsAndActions.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace Models.NPCScripts.Enemy
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Enemy : MonoBehaviour, ISetDamage, IGetConditions
    {
        public delegate void SeeEnemy(string unitName);

        public delegate void TakeDamage(float dmg, string unitName);

        private Transform _transform;
        private NavMeshAgent agent;

        private BaseConditions conditions;


        private EnemyController controller;
        private CapsuleCollider enemyBorder;
        private EnemyHealthBarUI healthBar;
        private SphereCollider enemyView;
        private MeshRenderer gun;
        private Transform gunBarrelEnd;
        private AudioSource gunShotSound;
        private Transform head;
        private MeshRenderer headMesh;
        private MeshRenderer knife;
        private MeshRenderer mesh;
        private GameObject player;
        private Rigidbody rb;
        private LineRenderer shootLine;
        private ParticleSystem bloodSplash;
        [SerializeField] private EnemySpecifications specification;

        /// <summary>
        ///     Метод применения состояний к персонажу
        /// </summary>
        /// <param name="Characteristics">Массив состояний</param>
        public void ApplyCondition(params CurrentCondition[] Characteristics)
        {
            foreach (var Condition in Characteristics)
                if (conditions.Conditions.HasName(Condition.Name))
                    ConditionChance(Condition);
        }

        public void ApplyDamage(float damage)
        {
            bloodSplash.Play();
            DamageEvent(damage, _transform.name);
        }

        public static event SeeEnemy SeeEvent;
        public static event TakeDamage DamageEvent;

        public void SetDieMethod(EnemyDie.DieContainer Method)
        {
            controller.Dying.DieEvent += Method;
        }

        public void EnemyAwake()
        {
            _transform = GetComponent<Transform>();
            head = _transform.GetChild(0);
            mesh = GetComponent<MeshRenderer>();
            headMesh = head.GetComponent<MeshRenderer>();
            gun = _transform.GetChild(1).GetComponent<MeshRenderer>();
            knife = _transform.GetChild(3).GetComponent<MeshRenderer>();
            knife.enabled = false;
            gunBarrelEnd = _transform.GetChild(2);
            gunShotSound = gunBarrelEnd.GetComponent<AudioSource>();
            headMesh.material.color = new Color(1, 0.9058824f, 0.6745098f, 1);
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            enemyBorder = GetComponent<CapsuleCollider>();
            healthBar = GetComponent<EnemyHealthBarUI>();
            enemyView = GetComponent<SphereCollider>();
            shootLine = GetComponentInChildren<LineRenderer>();
            player = GameObject.FindGameObjectWithTag("Player");
            enemyView.radius = specification.ViewDistance;
            bloodSplash = this.agent.GetComponent<ParticleSystem>();
            conditions = new BaseConditions(specification.GetCharacterConditionsList(),
                new CharacterConditionsSpecifications(specification));

            controller = new EnemyController(_transform, agent, mesh, headMesh, gun, knife, gunBarrelEnd, rb,
                enemyBorder,
                enemyView, shootLine, specification, _transform.position, player, gunShotSound, conditions, healthBar);
            controller.EnemyControllerAwake();
        }

        public void EnemyUpdate(float deltaTime)
        {
            controller.EnemyControllerUpdate(deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player) SeeEvent(_transform.name);
        }

        /// <summary>
        ///     Рассчет получения конкретного статуса
        /// </summary>
        /// <param name="Condition"></param>
        private void ConditionChance(CurrentCondition Condition)
        {
            //НЕТ РОЛЕВОЙ СИСТЕМЫ!!!

            conditions.Conditions.ChangeConditionStatus(Condition.Name, true);
        }
    }
}