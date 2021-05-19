using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace BeastHunter
{
    public class SporesController : MonoBehaviour
    {
        public List<GameObject> SporeList = new List<GameObject>();
        public GameObject PoisonCloud;
        public GameObject Puff;
        public GameObject Death;
        private float _time;
        private bool _isPuf;
        private float _delay = 3f;
        private float _damage;

        private void Start()
        {
            _time = 5f;
            transform.position += Vector3.down;
            var num = Random.Range(0, SporeList.Count);
            SporeList[num].SetActive(true);
            transform.DOLocalMoveY(transform.position.y + 0.9f, 1);
        }

        private void Update()
        {
            _time -= Time.deltaTime;
            if(_time<= 4)
            {
                Puff.SetActive(true);
            }
            if (_time <= 3.5f)
            {
                PoisonCloud.SetActive(true);
            }
            if (_time <= 1.5f)
            {
                if (!_isPuf)
                {
                    _isPuf = true;
                    transform.DOLocalMoveY(transform.position.y-1f, 5);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                _delay -= Time.deltaTime;
                if (_delay <= 0)
                {
                    _delay = 3f;
                    Damage(other);
                }
            }
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
        private void Damage(Collider enemy)
        {
            Damage damage = new Damage();
            damage.ElementDamageType = ElementDamageType.Toxin;
            damage.ElementDamageValue = Random.Range(15f, 30f);
            damage.PhysicalDamageType = PhysicalDamageType.Cutting;
            damage.PhysicalDamageValue = 0;

            Services.SharedInstance.AttackService.CountAndDealDamage(damage, 
                enemy.transform.root.gameObject.GetInstanceID());
        }
    }
}