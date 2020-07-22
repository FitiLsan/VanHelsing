using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class PlaceEnterHandler : MonoBehaviour
    {
        public GameObject Hub;
        public GameObject HubButton;
        public GameObject Map;

        private void Awake()
        {
            Hub = GameObject.Find("Hub");
            HubButton = GameObject.Find("HubButton");
            Map = GameObject.Find("Map");

            ToComeInPlace.ToComeInPlaceEvent += OnComeIn;
            PlaceExit.PlaceExitEvent += OnExit;
        }

        private void Start()
        {
            Hub.SetActive(false);
            HubButton.SetActive(false);
        }
        public void OnComeIn(int placeId)
        {
            // Только для входа в Хаб.
            Debug.Log($"entered to hub {placeId}");
            if(placeId.Equals(1))
            {
                Hub.SetActive(true);
                HubButton.SetActive(true);
                Map.SetActive(false);
            }
        }
        public void OnExit()
        {
            // Только для выхода из Хаба.
            Debug.Log($"exit from hub");
                Hub.SetActive(false);
                HubButton.SetActive(false);
                Map.SetActive(true);
        }
    }
}