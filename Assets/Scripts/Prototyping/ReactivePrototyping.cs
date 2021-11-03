using UnityEngine;
using UnityEngine.Events;
using Extensions;
using DG.Tweening;
using System.Collections;


namespace BeastHunter
{
    public sealed class ReactivePrototyping : MonoBehaviour
    {
        #region Fields

        private GameContext _context;
        [SerializeField] private bool _doCheckForPlayerOnly;
        [SerializeField] private UnityEvent _action;
        public GameObject _player;
        public bool _isInside;

        #endregion

        void Start()
        {
            if (_action == null)
            {
                _action = new UnityEvent();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == TagManager.PLAYER && !other.isTrigger)
            {
                _player = other.gameObject;
                _isInside = true;
                _action.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == TagManager.PLAYER && !other.isTrigger)
            {
                _isInside = false;
            }
        }

        public void RPMoveUp(float value)
        {
            gameObject.transform.Translate(0f, value, 0f);
        }

        public void RPMoveForward(float value)
        {
            gameObject.transform.Translate(0f, 0f, value);
        }

        public void RPMoveRihgt(float value)
        {
            gameObject.transform.Translate(value, 0f, 0f);
        }

        public void RPMoveUpSmooth(float value)
        {
            Sequence newSequence = DOTween.Sequence();
            newSequence.Append(transform.DOMove(transform.position + Vector3.up * value, 5f)).SetEase(Ease.InOutFlash);
            newSequence.Join(transform.DORotate(transform.eulerAngles + Vector3.right * 90f, 5f));
            newSequence.OnComplete(() => RPDestroyObject(0f));
            
        }

        public void RPDealDamageToPlayer(float damage)
        {
            Damage newDamage = new Damage();
            newDamage.PhysicalDamageValue = damage;
            Services.SharedInstance.AttackService.DealDamage(newDamage,Services.SharedInstance.Context.CharacterModel.InstanceID);
        }

        public void RPDestroyObject(float time)
        {
            Destroy(gameObject, time);
        }

        public void RPMovePlayer(float value)
        {
            _player.transform.Translate(0f, value, -value);
        }

        public void RPPushPlayer(float value)
        {
            Rigidbody rb = _player.GetComponent<Rigidbody>();
            rb.AddForce((Vector3.up - _player.transform.forward) * value, ForceMode.Impulse);
        }

        public void RPDamageZone(float value)
        {
            StartCoroutine(DamageOverTime(value));
        }

        IEnumerator DamageOverTime(float value)
        {
            while (_isInside)
            {
                Damage newDamage = new Damage();
                newDamage.PhysicalDamageValue = value;
                Services.SharedInstance.AttackService.DealDamage(newDamage, Services.SharedInstance.Context.CharacterModel.InstanceID);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}




