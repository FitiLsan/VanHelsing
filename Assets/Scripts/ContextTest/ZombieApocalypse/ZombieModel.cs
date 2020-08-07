using UnityEngine;

<<<<<<< HEAD

=======
>>>>>>> 9ab8e797e9b630800ee6901a5c40143f11274fc7
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