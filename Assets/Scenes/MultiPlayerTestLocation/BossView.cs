using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossView : MonoBehaviour, IPunObservable
{
    private PhotonView _photonView;
    private Animator _animator;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }


    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {


    }
}
