using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class SporesController : MonoBehaviour
    {
        public List<GameObject> sporeList = new List<GameObject>();

        private void Awake()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                sporeList.Add(transform.GetChild(i).gameObject);
            }
        }

        private void Start()
        {
            var num = Random.Range(0, sporeList.Count);

            sporeList[num].SetActive(true);
        }
    }
}