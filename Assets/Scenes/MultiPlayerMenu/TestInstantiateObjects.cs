using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace BeastHunter
{
    public class TestInstantiateObjects : MonoBehaviour
    {
        public GameObject prefab;


        void Start()
        {
            PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}