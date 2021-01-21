using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeastHunter
{
    public class TentacleController : MonoBehaviour, IPointerClickHandler
    {
        private Animator _animator;
        private bool _isCatched;

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
            _animator = transform.root.GetComponent<Animator>();
        }

        private void Catch()
        {
            _animator.SetTrigger("catch");
            _isCatched = true;
            _animator.SetBool("isCatched", _isCatched);
            _animator.SetBool("isCatched", _isCatched);
        }

        private void Throw()
        {
            _animator.SetTrigger("throw");
            _isCatched = false;
            _animator.SetBool("isCatched", _isCatched);
        }

        public void CatchedEvent()
        {
            Debug.Log("CatchedEVENT");
        }
    }
}