using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public class CallOfForestController : MonoBehaviour
    {
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private Transform _target;
        private bool _canAttack;
        private bool _startDeath;
        private bool _isMove;
        private bool _isAttack;
        private float _attackRange = 2f;

        public ParticleSystem BornEffect;
        public ParticleSystem DeathEffect;
        public GameObject Hand;
        

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
       
        }

        private void Start()
        {
            Hand.GetComponent<HitBoxTrigger>().HitActionEvent += HitDamage;
            _animator.Play("IdleState", 0, 0);
            BornEffect.Play();
            _isAttack = false;
            DG.Tweening.DOVirtual.DelayedCall(10f, DeadByTime);
            DG.Tweening.DOVirtual.DelayedCall(2f, AfterStart);
            
        }

        private void Update()
        {
            if (_target != null && !_startDeath)
            {
                _canAttack = CheckIsNearTarget(transform.position, _target.position, _attackRange);
                if (_canAttack)
                {
                    if (!_isAttack)
                    {
                        _isAttack = true;
                        _isMove = false;
                        _navMeshAgent.isStopped = true;
                        _animator.Play("BossFeastsAttack_0", 0, 0);
                        _animator.SetFloat("Speed", 0);
                        DG.Tweening.DOVirtual.DelayedCall(1.5f, () => _isAttack = false);
                    }
                }
                else if (!_isMove)
                {
                    _isMove = true;

                    _animator.SetFloat("Speed", 4f);
                    _animator.Play("MovingState", 0, 0);
                }
                else
                {
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(_target.position);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnDisable()
        {
            Hand.GetComponent<HitBoxTrigger>().HitActionEvent -= HitDamage;
        }
        private bool CheckIsNearTarget(Vector3 prefabPosition, Vector3 targetPosition, float distanceRange)
        {
            var isNear = Mathf.Sqrt((prefabPosition - targetPosition).sqrMagnitude) <= distanceRange;
            return isNear;
        }

        private void AfterStart()
        {
            var List = Services.SharedInstance.PhysicsService.GetObjectsInRadiusByTag(transform.position, 20f, "Player");
            foreach (var obj in List)
            {
                var trigger = obj.GetComponent<ITrigger>();
                if (trigger != null && trigger.Type == InteractableObjectType.Player)
                {
                    _target = obj.transform;
                }
            }
            if (_target != null)
            {
                _navMeshAgent.SetDestination(_target.position);
                _navMeshAgent.stoppingDistance = 1.9f;
                
                _animator.SetFloat("Speed", 4f);
                _animator.Play("MovingState", 0, 0);
            }

        }
        private void DeadByTime()
        {
            _startDeath = true;
            _navMeshAgent.isStopped = true;
            DeathEffect.Play(true);
            _animator.Play("DeadState",0,0);
            Destroy(gameObject, 7f);
            
        }

        private void HitDamage()
        {
            var damage = new Damage();
            damage.ElementDamageType = ElementDamageType.None;
            damage.ElementDamageValue = Random.Range(15f, 30f);
            damage.PhysicalDamageType = PhysicalDamageType.Cutting;
            damage.PhysicalDamageValue = Random.Range(15f, 30f);
            Services.SharedInstance.AttackService.CountAndDealDamage(damage, _target.transform.root.gameObject.GetInstanceID()); ;
        }
    }
}