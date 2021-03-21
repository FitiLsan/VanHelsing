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
        private Transform _catchedTargetRoot;
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
            if (other.tag.Equals("Player") && !other.isTrigger)
            {
                _fabrik.solver.target = other.transform;
                _target = other.gameObject;
               // Click();
            }
        }

        private void Update()
        {
            if(!_isGrowEnd)
            {
                return;
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                Click();
            }
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
            _catchedTargetRoot.SetParent(null);
            rb.AddForce(Vector3.forward * 30f, ForceMode.Impulse);
         //   _canLookAt = true;
            
        }

        public void OnCatchedEvent(GameObject bone, GameObject catchedTarget)
        {
            if (_isStartCatch && !_isCatched)
            {
                _isCatched = true;
                _catchedTarget = catchedTarget;
                _catchedTargetRoot = catchedTarget.transform.root.transform;
                _catchedTarget.GetComponent<Rigidbody>().isKinematic = true;
                _catchedTargetRoot.position = CatchPoint.position + new Vector3(0, -1, 0);
                _catchedTargetRoot.rotation = CatchPoint.rotation;
                _catchedTargetRoot.SetParent(CatchPoint);
                _canLookAt = false;
                _animator.SetBool("isCatched", _isCatched);
                //DOVirtual.DelayedCall(3f, Click);
            }
        }

        private void RotateToTarget()
        {
            if (_canLookAt)
            {
                if (_target != null)
                {
                    //var target = new Vector3(_target.transform.position.x, 0, _target.transform.position.z);
                    _root.LookAt(_target.transform);
                }
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