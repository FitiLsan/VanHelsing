using UnityEngine;

namespace Extensions
{
    public static partial class Vector3Extensions
    {
        public static Vector3 ClampSimmetrical(this Vector3 value, Vector3 center, 
            float marginX, float marginY, float marginZ)
        {
            return new Vector3(Mathf.Clamp(value.x, center.x - marginX, center.x + marginX), 
                Mathf.Clamp(value.y, center.y - marginY, center.y + marginY),
                    Mathf.Clamp(value.z, center.z - marginZ, center.z + marginZ));
        }

        public static Vector3 ClampNonSimmetrical(this Vector3 value, Vector2 xMargins, Vector2 yMargins, Vector2 zMargins)
        {
            if(xMargins.y < xMargins.x || yMargins.y < yMargins.x || zMargins.y < zMargins.x)
            {
                throw new System.Exception("wrong margin parameters - x element must be less then y");
            }

            return new Vector3(Mathf.Clamp(value.x, xMargins.x, value.y),
                Mathf.Clamp(value.y, yMargins.x, yMargins.y),
                    Mathf.Clamp(value.z, zMargins.x, zMargins.y));
        }

        public static Vector3 GetRoundPosition(this Vector3 value)
        {
            value.x = Mathf.Round(value.x * 100f)/100f;
            value.y = Mathf.Round(value.y * 100f)/100f;
            value.z = Mathf.Round(value.z * 100f)/100f;
            return value;
        }
        
        public static Vector3 MultiplyX(this Vector3 v, float val)
        {
            v = new Vector3(val * v.x, v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyY(this Vector3 v, float val)
        {
            v = new Vector3(v.x, val * v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyZ(this Vector3 v, float val)
        {
            v = new Vector3(v.x, v.y, val * v.z);
            return v;
        }
    }
}
