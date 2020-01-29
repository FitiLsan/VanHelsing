using UnityEngine;

namespace Models
{
    internal class CameraModel : MonoBehaviour
    {
        [SerializeField] public float AxisX_MouseSensivity = 3f;

        [SerializeField] public float AxisY_MouseSensivity = 3f;

        [SerializeField] public float CameraMaxDistance = 15f;
        [SerializeField] public float CameraMinDistance = 4.0f;

        [SerializeField] public float CameraMoveSpeed = 5;

        [SerializeField] public float CameraObstacleAvoidSpeed = 5;

        [SerializeField] public float CameraReturnSpeed = 10;

        [SerializeField] public float CameraZoomSpeed = 300f;

        [SerializeField] public float DistanceFromObstacle = 0.25f;

        [SerializeField] public LayerMask Mask;
    }
}