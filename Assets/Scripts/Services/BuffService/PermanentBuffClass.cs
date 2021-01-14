using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewPermanentBuff", menuName = "Effects/PermanentBuff")]
    public sealed class PermanentBuffClass : ScriptableObject
    {
        #region Fields

        public string Name;
        public string Description;
        public BuffType Type;
        public Sprite Sprite;
        public BuffEffect[] Effects;

        #endregion
    }
}

