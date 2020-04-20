using UnityEngine;
using System;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CharacterData")]
    public sealed class CharacterData : ScriptableObject
    {
        #region PrivateData

        public CharacterCommonSettingsStruct _characterCommonSettings;
        public CharacterCameraStruct _characterCameraSettings;
        public Action Hit { get; set; }

        #endregion


        #region Fields

        private Vector3 _movementVector;
        private float _currentAngleVelocity;

        #endregion


        #region Metods

        public void MoveForward(Transform prefabTransform, float moveSpeed)
        {
            _movementVector = Vector3.forward * moveSpeed * Time.deltaTime;
            prefabTransform.Translate(_movementVector, Space.Self);
        }

        public void Move(Transform prefabTransform, float moveSpeed, Vector3 direction)
        {
            _movementVector = direction * moveSpeed * Time.deltaTime;
            prefabTransform.Translate(_movementVector, Space.Self);
        }

        public void JumpForward(Rigidbody rigitbody, float horizontalForce, float verticalForce, float currentSpeed)
        {
            rigitbody.AddForce(rigitbody.transform.forward * 
                (horizontalForce + currentSpeed), ForceMode.Impulse);
            rigitbody.AddForce(Vector3.up * verticalForce, ForceMode.Impulse);
        }

        public void Dodge(Rigidbody rigidbody, float force, Vector3 direction)
        {
            //TODO
        }

        public void OnHit()
        {
            Debug.Log("hit");
            if(Hit != null)
            {
                Hit.Invoke();
            }
        }

        #endregion
    }
}

