using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterSpeedCounter
    {
        #region Fields

        private float _targetSpeed;
        private float _speedChangeLag;

        #endregion


        #region Properties

        public float WalkSpeed { get; }
        public float RunSpeed { get; }
        public float SpeedAccelerationLag { get; }
        public float SpeedDecelerationLag { get; }
        public float MinimalSpeed { get; }

        #endregion


        #region ClassLifeCycle

        public CharacterSpeedCounter(float walkSpeed, float runSpeed, float speedAccelerationLag, 
            float speedDecelerationLag, float minimalSpeed)
        {
            WalkSpeed = walkSpeed;
            RunSpeed = runSpeed;
            SpeedAccelerationLag = speedAccelerationLag;
            SpeedDecelerationLag = speedDecelerationLag;
            MinimalSpeed = minimalSpeed;
        }

        #endregion


        #region Methods

        public void CountSpeed(bool isMoving, bool isRunning, ref float currentSpeed, ref float velocity)
        {
             _targetSpeed = isMoving ? (isRunning ? RunSpeed : WalkSpeed) : 0f;

            if(currentSpeed <= _targetSpeed)
            {
                _speedChangeLag = SpeedAccelerationLag;
                GetSpeed(ref currentSpeed, ref velocity);
            }
            else if(currentSpeed > MinimalSpeed)
            {
                _speedChangeLag = SpeedDecelerationLag;
                GetSpeed(ref currentSpeed, ref velocity);
            }
            else
            {
                currentSpeed = 0f;
            }       
        }

        private void GetSpeed(ref float currentSpeed, ref float velocity)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, _targetSpeed, ref velocity, _speedChangeLag);
        }

        #endregion
    }
}

