using UnityEngine;


namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Rabbit", order = 1)]
    public sealed class RabbitData : ScriptableObject
    {
        #region Fields

        private const float ROTATION_SPEED = 5.0f;
        private const float HOP_FORCE_MULTIPLIER = 100.0f;
        private const float MAX_ANGLE_DEVIATION = 40.0f;

        public RabbitStruct RabbitStruct;

        #endregion


        #region Metods

        public bool CheckIfOnGround(Transform transform)
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.0f, ~LayerMask.NameToLayer("Ground"));
        }

        public void RotateTo(Transform transform, Vector3 from, Vector3 to, out bool rotationFinished)
        {
            float singleStep = ROTATION_SPEED * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(from, to, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            float dot = Vector3.Dot(from.normalized, to.normalized);
            rotationFinished = dot >= 1.0f;
        }

        public void Hop(Rigidbody rigidbody, Vector3 direction, float acceleration)
        {
            rigidbody.AddForce((direction * RabbitStruct.MoveSpeed * acceleration + Vector3.up * RabbitStruct.JumpHeight) * HOP_FORCE_MULTIPLIER);
        }

        public Vector3 ReturningNextCoord(Transform transform, Vector3 startPosition) // NextCoord and ReturningAngle + RandomAngle
        {
            var next = (startPosition - transform.position).normalized;
            //var distance = (transform.position - startPosition).magnitude;
            //var angle = AngleDeviation(distance);

            //var distance = (transform.position - startPosition).sqrMagnitude;
            //var angle = AngleDeviationSqr(distance);

            var distance = (transform.position - startPosition).sqrMagnitude;
            var angle = AngleDeviationSqrPlain(distance);

            angle = Random.Range(-angle, angle);
            angle *= Mathf.Deg2Rad;
            var forward = new Vector2(next.x, next.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        public float AngleDeviation(float distance) // linear, use with distance magnitude
        {
            var R = RabbitStruct.RunningRadius;
            var f = 3.0f;
            return (MAX_ANGLE_DEVIATION * f) / (f - 1.0f) * (-(distance / R) + 1);
        }

        public float AngleDeviationSqr(float distance) // parabolic, use with distance sqrMagnitude (variant 1, point on runningRadius/returnFactor)
        {
            var R = RabbitStruct.RunningRadius;
            var f = 3.0f;
            var a = (MAX_ANGLE_DEVIATION * f * f) / (R * R * (2.0f * f - f * f - 1.0f));
            var b = -(2.0f * a * R) / f;
            var c = (a * R * R * (2.0f - f)) / f;
            return a * distance * distance + b * distance + c;
        }

        public float AngleDeviationSqrPlain(float distance) // parabolic, use with distance sqrMagnitude (variant 2, point on startPos)
        {
            var R = RabbitStruct.RunningRadius;
            var f = 3.0f;
            var a = -((MAX_ANGLE_DEVIATION * f * f) / (R * R * (f * f - 1.0f)));
            var c = -a * R * R;
            return a * distance * distance + c;
        }

        public Vector3 RandomNextCoord(Transform transform, Vector3 startPosition) 
        {
            var direction = Random.Range(0.0f, 100.0f);
            var angle = 0f; // turn forward
            if (direction >= 80.0f)
            {
                angle = 90.0f; // turn left
            }
            else if (direction >= 60.0f)
            {
                angle = 270.0f; // turn right
            }
            else if (direction >= 45.0f)
            {
                angle = 180.0f; // turn back
            }
            angle += Random.Range(-MAX_ANGLE_DEVIATION, MAX_ANGLE_DEVIATION);
            angle *= Mathf.Deg2Rad;

            // Nearest neighbour distance < x => angle = nnd+180
            var forward = new Vector2(transform.forward.x, transform.forward.z);
            return new Vector3(RotateByAngle(forward, angle).x, transform.forward.y, RotateByAngle(forward, angle).y);
        }

        private Vector2 RotateByAngle(Vector2 vector, float angle)
        {
            return new Vector2(vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle), vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle));
        }

        #endregion
    }
}
