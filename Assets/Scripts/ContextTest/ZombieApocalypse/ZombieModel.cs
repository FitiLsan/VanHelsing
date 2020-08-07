using UnityEngine;


namespace BeastHunter
{
    public sealed class ZombieModel : TestCharacterModel
    {
        #region ClassLifeCycle

        public ZombieModel(GameObject prefab, TestCharacterData testCharacterData) : base(prefab, testCharacterData)
        {
            _transform.tag = TagManager.ENEMY;
        }

        #endregion
    }
}