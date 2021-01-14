using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData")]
    public sealed class CharacterData : ScriptableObject
    {
        #region Fields

        [SerializeField] private CharacterCommonSettings _characterCommonSettings;
        [SerializeField] private CharacterAnimationData _characterAnimationData;
        [SerializeField] private Stats _characterStatsSettings;

        private Vector3 _movementVector;

        #endregion


        #region Fields

        public CharacterCommonSettings CharacterCommonSettings => _characterCommonSettings;
        public CharacterAnimationData CharacterAnimationData => _characterAnimationData;
        public Stats CharacterStatsSettings => _characterStatsSettings;

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

