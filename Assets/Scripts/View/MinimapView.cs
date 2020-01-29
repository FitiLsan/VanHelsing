using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{    public class MinimapView : MonoBehaviour
    {
        private GameObject _mainCamera;
        private GameObject _player;
        private Vector3 _mainCamRotation;
        private Vector3 pos;
       
        void Start()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void MinimapUpdate()
        {
            transform.position = new Vector3(_player.transform.position.x, 150, _player.transform.position.z);
            _mainCamRotation = _mainCamera.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(90, _mainCamera.transform.rotation.eulerAngles.y, 0);
        }

        void Update()
        {
            MinimapUpdate();            
        }
    }
}

