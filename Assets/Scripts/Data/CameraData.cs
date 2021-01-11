using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "CameraData")]
    public sealed class CameraData : ScriptableObject
    {
        #region FIelds

        public CharacterCameraSettings _cameraSettings;

        #endregion
    }
}

