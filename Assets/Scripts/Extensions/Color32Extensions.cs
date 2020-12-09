using UnityEngine;


namespace Extensions
{
    public static partial class Color32Extensions
    {
        public static Color32 RandomColor => new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);

        public static Color32 SetA(this Color32 c, byte a)
        {
            c.a = a;
            
            return c;
        }
        
        public static string ToHex(this Color32 c) => $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
    }
}
