using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Character/CameraData")]
    public sealed class CameraData : ScriptableObject
    {
        #region FIelds

        public CharacterCameraSettings _cameraSettings;

        #endregion
    }
}

