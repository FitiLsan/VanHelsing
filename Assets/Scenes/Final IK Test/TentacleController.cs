using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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
        private GameObject _catchedTarget;
        private Sequence sequence;
        private bool _isGrowEnd;

        public Transform CatchPoint;
        public float xOffset;
        public float zOffset;
        public float time;


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
            _target = _fabrik.solver.target.gameObject;
            _weight = _fabrik.solver.GetIKPositionWeight();
        }

        private void Start()
        {
            transform.position += new Vector3(0, -5.3f, 0);
            transform.DOLocalMoveY(0, 3f);
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMoveY(0, 4f)).AppendCallback(GrowingEnd);
            
        }

        private void OnDisable()
        {
            CatchTrigger.CatchedEvent -= OnCatchedEvent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _fabrik.solver.target = other.transform;
            }
        }

        private void Update()
        {
            if(!_isGrowEnd)
            {
                return;
            }
            _target = _fabrik.solver.target.gameObject;

            RotateToTarget();

            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Click();
            }
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
                _animator.SetTrigger("throw");
                _isCatched = false;
                _animator.SetBool("isCatched", _isCatched);
                _flag = false;
                DOVirtual.DelayedCall(0.8f, SwitchKinematic);
            }
        }

        private void SwitchKinematic()
        {
            var rb = _catchedTarget.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * 30f, ForceMode.Impulse);
            _canLookAt = true;
            _catchedTarget.transform.parent = null;
        }

        public void OnCatchedEvent(GameObject bone, GameObject catchedTarget)
        {
            if (_isStartCatch)
            {
                _isCatched = true;
                _catchedTarget = catchedTarget;
                _catchedTarget.GetComponent<Rigidbody>().isKinematic = true;
                catchedTarget.transform.SetParent(CatchPoint);
                var pos = new Vector3(CatchPoint.position.x, CatchPoint.position.y, CatchPoint.position.z - zOffset);
                catchedTarget.transform.position = CatchPoint.position;
                _canLookAt = false;
                _animator.SetBool("isCatched", _isCatched);
            }
        }

        private void RotateToTarget()
        {
            if (_canLookAt)
            {
                var target = new Vector3(_target.transform.position.x, 0, _target.transform.position.z);
                _root.LookAt(target);               
            }
        }

        public void Click()
        {
            if (!_isCatched)
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