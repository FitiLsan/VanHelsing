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
        private GameObject _target;
        private float _weight;
        private FABRIK _fabrik;
        private bool _flag;
        private bool _canLookAt = true;
        private GameObject _catchingBone;
        private GameObject _catchedTarget;

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
            CatchTrigger.ThrowEvent += OnThrowEvent;

            _animator = transform.root.GetComponent<Animator>();
            _root = transform.root;
            _fabrik = transform.root.GetComponent<FABRIK>();
            _target = _fabrik.solver.target.gameObject;
            _weight = _fabrik.solver.GetIKPositionWeight();
        }

        private void OnDisable()
        {
            CatchTrigger.CatchedEvent -= OnCatchedEvent;
            CatchTrigger.ThrowEvent -= OnThrowEvent;
        }

        private void Update()
        {
            RotateToTarget();

            _target = _fabrik.solver.target.gameObject;

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

          //  _fabrik.solver.SetIKPositionWeight(_weight);
        }

        private void Catch()
        {
            _animator.SetTrigger("catch");
            _isCatched = true;
            _animator.SetBool("isCatched", _isCatched);
        }

        private void Throw()
        {
            _animator.SetTrigger("throw");
            _isCatched = false;
            _animator.SetBool("isCatched", _isCatched);
            _flag = false;
            DOVirtual.DelayedCall(0.8f, SwitchKinematic);
        }

        private void SwitchKinematic()
        {
            _catchedTarget.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Kinematic off");
        }

        public void OnCatchedEvent(GameObject bone, GameObject catchedTarget)
        {
            if (_isCatched)
            {
                _catchingBone = bone;
                _catchedTarget = catchedTarget;
                _catchedTarget.GetComponent<Rigidbody>().isKinematic = true;
                catchedTarget.transform.SetParent(CatchPoint);
                var pos = new Vector3(CatchPoint.position.x, CatchPoint.position.y, CatchPoint.position.z - zOffset);
                catchedTarget.transform.position = CatchPoint.position;
                _canLookAt = false;
            }
        }

        private void OnThrowEvent()
        {
            if (!_isCatched)
            {
                _canLookAt = true;
                _catchedTarget.transform.parent = null;
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
                Debug.Log("Catch");
                Catch();
            }
            else if (_isCatched)
            {
                Debug.Log("Throw");
                Throw();
            }
        }

    }
}