using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using RootMotion.Dynamics;

namespace BeastHunter
{
    public class TentacleController : MonoBehaviour, IPointerClickHandler
    {
        private Animator _animator;
        private Transform _root;
        private bool _isCatched;
        private bool _isStartCatch;
        private GameObject _target;
        private float _weight;
        private FABRIK _fabrik;
        private bool _flag;
        private bool _canLookAt = true;
        private Transform _catchedTarget;
        private Transform _catchedTargetRoot;
        private Sequence sequence;
        private bool _isGrowEnd;
        private PuppetMaster _puppetMaster;
        private bool _usedOnce;
        private Rigidbody _targetRigidbody;
        public Transform CatchPoint;
        public float xOffset;
        public float zOffset;
        public float time;
        public Collider[] colliders;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isCatched)
            {
                Debug.Log("Catch");
                Catch();
            }
            else if(_isCatched)
            {
                Debug.Log("Throw");
                Throw();
            }
        }

        private void Awake()
        {
            CatchTrigger.CatchedEvent += OnCatchedEvent;

            _animator = transform.root.GetComponent<Animator>();
            _root = transform.root;
            _fabrik = transform.root.GetComponent<FABRIK>();
          //  _target = _fabrik.solver.target.gameObject;
            _weight = _fabrik.solver.GetIKPositionWeight();

        }

        private void Start()
        {
           transform.localPosition += new Vector3(0, -5.3f, 0);
           sequence = DOTween.Sequence();
           sequence.Append(transform.DOLocalMoveY(transform.position.y + 5.3f, 4f)).AppendCallback(GrowingEnd);
        }

        private void OnDisable()
        {
            CatchTrigger.CatchedEvent -= OnCatchedEvent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player") && !other.isTrigger && !_usedOnce)
            {

                foreach (var Coll in colliders)
                {
                    Coll.enabled = true;
                }
                _canLookAt = true;
                _fabrik.solver.target = other.transform;
                _target = other.gameObject;
                Click();
            }
        }

        private void Update()
        {
            if(!_isGrowEnd)
            {
                return;
            }

            //if(Input.GetKeyDown(KeyCode.P))
            //{
            //    AddForceToTarget();
            //    //Click();
            //}

            RotateToTarget();

            if (_isCatched)
            {
                if (!_flag)
                {
                    _weight += time * Time.deltaTime;
                    if (_weight >= 1f)
                    {
                        _weight = 0;
                        _flag = true;
                    }
                }
            }
        }

        private void Catch()
        {
            _animator.SetTrigger("catch");
            _isStartCatch = true;
            DOVirtual.DelayedCall(2f, () => _isStartCatch = false);
        }

        private void Throw()
        {
            if (_isCatched)
            {

                _isCatched = false;
                _animator.SetTrigger("throw");
                _animator.SetBool("isCatched", _isCatched);
                _flag = false;

                foreach (var Coll in colliders )
                {
                    Coll.enabled = false;
                }
                DOVirtual.DelayedCall(0.83f, SwitchKinematic);
            }
        }

        private void SwitchKinematic()
        {
            _targetRigidbody.isKinematic = false;
            _puppetMaster.mode = PuppetMaster.Mode.Active;
            _catchedTarget.SetParent(_catchedTargetRoot);
            DOVirtual.DelayedCall(0.1f, AddForceToTarget);
            DOVirtual.DelayedCall(5f, () => _usedOnce = false);
        }

        private void AddForceToTarget()
        {
            foreach (var item in _puppetMaster.muscles)
            {
                item.rigidbody.AddExplosionForce(50, transform.position, 15, 1.5f, ForceMode.Impulse);
            } 
        }


        public void OnCatchedEvent(GameObject bone, GameObject catchedTarget)
        {
            if (_isStartCatch && !_isCatched)
            {
                _isCatched = true;
                _usedOnce = true;
                _catchedTargetRoot = catchedTarget.transform.root;
                _puppetMaster = _catchedTargetRoot.GetComponentInChildren<PuppetMaster>();
                _puppetMaster.mode = PuppetMaster.Mode.Kinematic;

                _catchedTarget = _catchedTargetRoot.GetChild(2);
                _targetRigidbody = _catchedTarget.GetComponent<Rigidbody>();
                _targetRigidbody.isKinematic = true;
                _catchedTarget.position = CatchPoint.position + new Vector3(0, -1, 0);
                _catchedTarget.rotation = CatchPoint.rotation;
                _catchedTarget.SetParent(CatchPoint);
                _canLookAt = false;
                _animator.SetBool("isCatched", _isCatched);
               
                DOVirtual.DelayedCall(3f, Click);
            }
        }

        private void RotateToTarget()
        {
            if (_target != null && _canLookAt)
            {
                var target = new Vector3(_target.transform.position.x, _root.position.y, _target.transform.position.z);
                _root.LookAt(target);
            }
        }

        public void Click()
        {
            if (!_isCatched && !_isStartCatch)
            {
                Catch();
            }
            else if (_isCatched)
            {
                Throw();
            }
        }

        private void GrowingEnd()
        {
            _isGrowEnd = true;
        }
    }
}