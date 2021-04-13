using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeastHunter
{
    public class Ruler : MonoBehaviour
    {
        public List<GameObject> RulerRanges;
        public GameObject Ranges;
        public GameObject Image;
        public Text text;
        public int num;
        private void Start()
        {
            num = 0;
            for(var i=0; i< Ranges.transform.childCount;i++)
            {
                RulerRanges.Add(Ranges.transform.GetChild(i).gameObject);
                RulerRanges[i].SetActive(false);
            }
            text = Image.GetComponentInChildren<Text>();
        }

        public void NextRange()
        {
            Image.SetActive(true);
            if (num > RulerRanges.Count-1)
            {
                foreach (var r in RulerRanges)
                {
                    r.SetActive(false);
                }
                num = 0;
            }

            RulerRanges[num].SetActive(true);
            SetText(num);
            if (num > 0)
            {
                RulerRanges[num - 1].SetActive(false);
            }
            num++;
        }

        private void SetText(int num)
        {
            text.text = $"{RulerRanges[num].gameObject.name}";
            if(num==0)
            {
                Image.SetActive(false);
            }
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.RightAlt))
            //{
            //    NextRange();
            //}

        }
    }
}