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
        private bool isPuf;

        private void Start()
        {
            time = 5f;
            transform.position += Vector3.down;
            var num = Random.Range(0, sporeList.Count);
            sporeList[num].SetActive(true);
            transform.DOLocalMoveY(transform.position.y + 0.9f, 1);
        }

        private void Update()
        {
            time -= Time.deltaTime;
            if(time<= 4)
            {
                puff.SetActive(true);
            }
            if (time <= 3.5f)
            {
                poisonCloud.SetActive(true);
            }
            if (time <= 1.5f)
            {
                if (!isPuf)
                {
                    isPuf = true;
                    transform.DOLocalMoveY(transform.position.y-1f, 5);
                }
            }
        }
    }
}