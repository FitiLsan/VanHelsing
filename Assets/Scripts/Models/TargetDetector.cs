using UnityEngine;

namespace Models
{
    public class TargetDetector : MonoBehaviour
    {
        public Collider target;

        private void OnTriggerEnter(Collider collider)
        {
            target = collider;
            Debug.Log($"Detect TARGETDETECTOR => {target}");
        }

        private void OnTriggerExit(Collider collider)
        {
            target = null;
            Debug.Log("Detect TARGET LOST ");
        }
    }
}