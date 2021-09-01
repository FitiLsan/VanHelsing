using UnityEngine;
using RootMotion.Dynamics;
using Photon.Pun;

namespace BeastHunter
{
    public sealed class InitializeTrapController : MonoBehaviourPun
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
            //GameObject newTrapInHands = GameObject.Instantiate(_trapData.TrapStruct.TrapPrefabInHands);
            GameObject newTrapProjection = GameObject.Instantiate(_trapData.TrapStruct.TrapPrefabProjection,
                _characterModel.CharacterTransform);

            //TODO JEVLOGIN - Added Photon code
           
            var newTrapInHands2 =  PhotonNetwork.Instantiate(_trapData.TrapStruct.TrapPrefabInHands.name, 
                _characterModel.CharacterTransform.position, _characterModel.CharacterTransform.rotation);

            _characterModel.PuppetMaster.propMuscles[0].currentProp = newTrapInHands2.
                GetComponent<PuppetMasterProp>();

            _trapModel = new TrapModel(newTrapInHands2, newTrapProjection, _trapData);

            //_trapModel = new TrapModel(newTrapInHands, newTrapProjection, _trapData);
            _characterModel.CurrentPlacingTrapModel.Value = _trapModel;
        }

        #endregion
    }
}
