using UnityEngine;


namespace BeastHunter
{
    public sealed class CharacterInitilizeController : IAwake
    {
        #region Field

        GameContext _context;
        Services _services;

        #endregion


        #region ClassLifeCycle

        public CharacterInitilizeController(GameContext context, Services services)
        {
            _context = context;
            _services = services;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var characterData = Data.CharacterData;

            Vector3 instantiatePosition = characterData._characterStruct.InstantiatePosition;
            Vector3 groundedInstancePosition = GetGroundedPosition(instantiatePosition);

            GameObject instance = GameObject.Instantiate(characterData._characterStruct.Prefab);

            CharacterModel character = new CharacterModel(instance, characterData, groundedInstancePosition);
            _context._characterModel = character;
        }

        #endregion


        #region Methods

        private Vector3 GetGroundedPosition(Vector3 startPosition)
        {
            Vector3 groundedPosition = new Vector3();

            bool isGroundBelow = _services.PhysicsService.FindGround(startPosition, out groundedPosition);

            if (!isGroundBelow)
            {
                throw new System.Exception("Ground is above player's position!");
            }

            return groundedPosition;
        }

        #endregion
    }
}
