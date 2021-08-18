using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

namespace BeastHunter
{
    public class BossController : MonoBehaviour
    {
        private const float ANGULAR_MOVE_SPEED_REDUCTION_INDEX = 0.7f;

        private InputModel _inputModel;
        private GameContext _context;
        private Services _services;
        private bool _isInitialized;
        private PhotonView _photonView;

        private float AdditionalDirection { get; set; }

        void Start()
        {
            DOVirtual.DelayedCall(3f, Initialize);
        }

        private void Initialize()
        {
            _services = Services.SharedInstance;
            _context = _services.Context;
            _inputModel = _context.InputModel;
            _photonView = GetComponent<PhotonView>();
            _isInitialized = true;
        }

        void Update()
        {
            if (!_isInitialized || !_photonView.IsMine)
                return;
            Moving();
        }


        private void Moving()
        {
            if (_inputModel.IsInputMove)
            {
                Vector3 moveDirection = (Vector3.forward * _inputModel.InputAxisY + Vector3.right *
                    _inputModel.InputAxisX);

                if (Math.Abs(_inputModel.InputAxisX) + Math.Abs(_inputModel.InputAxisY) == 2)
                {
                    moveDirection *= ANGULAR_MOVE_SPEED_REDUCTION_INDEX;
                }

                Move(transform, 3, moveDirection);
            }
        }

        public void Move(Transform prefabTransform, float moveSpeed, Vector3 direction)
        {
            var _movementVector = direction * moveSpeed * Time.deltaTime;
            prefabTransform.Translate(_movementVector, Space.Self);
        }
    }
}