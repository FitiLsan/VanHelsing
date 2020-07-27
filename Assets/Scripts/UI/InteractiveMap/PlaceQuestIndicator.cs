using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class PlaceQuestIndicator : MonoBehaviour
    {
        public static event Action<List<Place>> PlacesScanEvent;
        public List<GameObject> placeGOList;
        public List<Place> placeList;

        private void Awake()
        {
            placeGOList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Place"));
            foreach(var place in placeGOList)
            {
                placeList.Add(place.GetComponent<Place>());
            }
        }

        private void Start()
        {
            PlacesScanEvent?.Invoke(placeList);
        }


        public void IndicatorOn()
        {
            foreach (var place in  placeList)
            {
              var npcList =  place.GetComponent<Place>().npcList;
                if(npcList.Count!=0)
                {
                   // npcList[0].
                }
            }
        }

    }
}