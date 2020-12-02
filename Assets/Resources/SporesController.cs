using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BeastHunter
{
    public class SporesController : MonoBehaviour
    {
        public List<GameObject> sporeList = new List<GameObject>();
        public GameObject poisonCloud;
        public GameObject puff;
        public GameObject death;
        private float time;
        Sequence sequence;// = DOTween.Sequence();

        private void Awake()
        {
            sequence = DOTween.Sequence();
            for (var i = 0; i < transform.childCount; i++)
            {
                sporeList.Add(transform.GetChild(i).gameObject);
            }
        }

        private void Start()
        {
            time = 3f;
            transform.position += Vector3.down;
            var num = Random.Range(0, sporeList.Count);
            sporeList[num].SetActive(true);
            transform.DOMoveY(0.6f, 1);
            sequence.PrependInterval(2f).Append(transform.DOMoveY(-1f, 2));
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if(time<= 2)
            {
                puff.SetActive(true);
               // transform.DOMoveY(-1f, 3);
            }
            if (time <= 1.5f)
            {
                poisonCloud.SetActive(true);
            }
        }
    }
}