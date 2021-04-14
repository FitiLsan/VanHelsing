using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewTemporaryBuff", menuName = "Effects/TemporaryBuff")]
    public sealed class TemporaryBuff :  BaseBuff
    {
        #region Fields

        public float Time;

        #endregion
    }
}
