using UnityEngine;


namespace Extensions
{
    public static partial class ColorExtensions
    {
        public static Color RandomColor
        {
            get { return new Color(Random.value, Random.value, Random.value); }
        }

        public static string ToHex(this Color c)
        {
            Color32 c32 = c;
            return c32.ToHex();
        }

        public static Color SetA(this Color c, float a)
        {
            c.a = a;

            return c;
        }
    }
}
