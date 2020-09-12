using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Example/InteractableObject", order = 1)]
    public sealed class IOData : EnemyData
    {
        #region Fields

        private PhysicsService _physicsService;

        #endregion


        #region Metods

        public void Act(IOModel io)
        {
            // adding service example
            if (_physicsService == null)
            {
                _physicsService = Services.SharedInstance.PhysicsService;
            }
            //-------
            if (io.CurrentColor != Color.red)
            {
                Move(io.IOTransform);
            }
            if (Random.Range(0.0f, 1.0f) > 0.8f)
            {
                ChangeColor(io);
            }

        }

        private void Move(Transform transform)
        {
            transform.Rotate(Vector3.right, 5f * Time.deltaTime);
        }

        private void ChangeColor(IOModel io)
        {
            var random = Random.Range(0.0f, 1.0f);
            if (random > 0.5f)
            {
                io.CurrentColor = Color.blue;
            }
            else
            {
                io.CurrentColor = Color.red;
            }

            io.IO.GetComponent<Renderer>().material.color = io.CurrentColor;
        }

        #endregion
    }

}
