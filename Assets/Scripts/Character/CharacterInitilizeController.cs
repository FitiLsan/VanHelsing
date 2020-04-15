using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterInitilizeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public CharacterInitilizeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var CharacterData = Data.CharacterData;
            GameObject instance = GameObject.Instantiate(CharacterData._characterStruct._prefab, 
                CharacterData._characterStruct.InstantiatePosition, Quaternion.identity);
            CharacterModel Character = new CharacterModel(instance, CharacterData);
            _context._characterModel = Character;
        }

        #endregion
    }
}
