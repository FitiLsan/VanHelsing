using UnityEngine;


namespace BeastHunter
{
    public class TrapService : IService
    {
        #region Fields

        private readonly GameContext _context;
        private TrapData _currentTrapData;

        #endregion


        #region ClassLifeCycles

        public TrapService(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public void GetTrap(TrapData trapData)
        {
            if (trapData.TrapsAmount > 0)
            {
                _currentTrapData = trapData;
                new InitializeTrapController(_context, trapData);
            }
        }

        public void PlaceTrap()
        {
            if(_currentTrapData != null)
            {
                _context.CharacterModel.CurrentPlacingTrapModel.Value.
                    TrapObjectInFrontOfCharacter.transform.parent = null;
                _currentTrapData.Place(_context, _context.CharacterModel.CurrentPlacingTrapModel.Value);
                GameObject.Destroy(_context.CharacterModel.CurrentPlacingTrapModel.Value.TrapObjectInHands);
                _context.CharacterModel.CurrentPlacingTrapModel.Value = null;
                _currentTrapData = null;
            }
        }

        public void RemoveTrap()
        {
            if(_currentTrapData != null)
            {
                _context.CharacterModel.PuppetMaster.propMuscles[0].currentProp = null;

                GameObject.Destroy(_context.CharacterModel.CurrentPlacingTrapModel.Value.TrapObjectInHands);
                GameObject.Destroy(_context.CharacterModel.CurrentPlacingTrapModel.Value.TrapObjectInFrontOfCharacter);

                _context.CharacterModel.CurrentPlacingTrapModel.Value = null;
                _currentTrapData = null;
            }
        }

        #endregion
    }
}

