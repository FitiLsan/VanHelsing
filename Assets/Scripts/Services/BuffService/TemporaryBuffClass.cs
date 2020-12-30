using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewTemporaryBuff", menuName = "Effects/TemporaryBuff")]
    public sealed class TemporaryBuffClass : ScriptableObject
    {
        #region Fields

        public string Name;
        public string Description;
        public float Time;
        public BuffType Type;
        public Sprite Sprite;
        public BuffEffect[] Effects;       

        #endregion
    }
}
