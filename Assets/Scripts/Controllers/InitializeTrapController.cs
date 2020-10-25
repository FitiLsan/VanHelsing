using UnityEngine;
using RootMotion.Dynamics;


namespace BeastHunter
{
    public sealed class InitializeTrapController
    {
        #region Field

        private readonly GameContext _context;
        private readonly TrapData _trapData;
        private readonly CharacterModel _characterModel;

        private TrapModel _trapModel;

        #endregion


        #region ClassLifeCycle

        public InitializeTrapController(GameContext context, TrapData trapData)
        {
            _context = context;
            _trapData = trapData;
            _characterModel = _context.CharacterModel;
            PrepareTrap();
        }

        #endregion


        #region Methods

        private void PrepareTrap()
        {
            GameObject newTrapInHands = GameObject.Instantiate(_trapData.TrapStruct.TrapPrefabInHands);
            GameObject newTrapProjection = GameObject.Instantiate(_trapData.TrapStruct.TrapPrefabProjection,
                _characterModel.CharacterTransform);

            _characterModel.PuppetMaster.propMuscles[0].currentProp = newTrapInHands.
                GetComponent<PuppetMasterProp>();

            _trapModel = new TrapModel(newTrapInHands, newTrapProjection, _trapData);
            _characterModel.CurrentPlacingTrapModel.Value = _trapModel;
        }

        #endregion
    }
}
