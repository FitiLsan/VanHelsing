using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CharacterData")]
    public sealed class CharacterData : ScriptableObject
    {
        #region PrivateData

        public CharacterCommonSettingsStruct _characterCommonSettings;
        public BaseStatsClass _characterStatsSettings;

        #endregion


        #region Fields

        private Vector3 _movementVector;

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

        #endregion
    }
}

