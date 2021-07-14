using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace BeastHunter
{
    public class TestInstantiateObjects : MonoBehaviour
    {
        public GameObject TestCubprefab;
        public GameObject TestEntprefab;
        
        


        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //  PhotonNetwork.Instantiate(TestCubprefab.name, Vector3.zero, Quaternion.identity);
                PhotonNetwork.Instantiate(TestEntprefab.name, Vector3.zero, Quaternion.identity);
            }
        }
    }
}