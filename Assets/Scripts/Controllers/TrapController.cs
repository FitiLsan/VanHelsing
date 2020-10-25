using UniRx;


namespace BeastHunter
{
    public sealed class TrapController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        private TrapModel _currentTrapModel;
        private float _trapPlacementDistance;
        private float _trapPlacementHeight;

        #endregion


        #region ClassLifeCycle

        public TrapController(GameContext context)
        {
            _context = context;
            _context.TrapModels = new System.Collections.Generic.Dictionary<int, TrapModel>();                
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            _context.CharacterModel.CurrentPlacingTrapModel.Subscribe(UpdateCurrentTrapPositionInformation);
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            UpdateCurrentTrapPlacementView();
        }

        #endregion


        #region ITearDown

        public void TearDown()
        {
            _context.CharacterModel.CurrentPlacingTrapModel.Dispose();
        }

        #endregion


        #region Methods

        private void UpdateCurrentTrapPositionInformation(TrapModel trapModel)
        {
            if(trapModel != null)
            {
                _currentTrapModel = trapModel;
                _trapPlacementDistance = _currentTrapModel.TrapStruct.ProjectionDistanceFromCharacter;
                _trapPlacementHeight = _currentTrapModel.TrapStruct.PlacingHeight;
            }
            else
            {
                _currentTrapModel = null;
                _trapPlacementDistance = 0f;
                _trapPlacementHeight = 0f;
            }
        }

        private void UpdateCurrentTrapPlacementView()
        {
            if (_currentTrapModel != null)
            {
                _currentTrapModel.TrapObjectInFrontOfCharacter.transform.position = Services.SharedInstance.
                    PhysicsService.GetGroundedPosition(_context.CharacterModel.CharacterTransform.position +
                        _context.CharacterModel.CharacterTransform.forward * _trapPlacementDistance) + _currentTrapModel.
                            TrapObjectInFrontOfCharacter.transform.up * _currentTrapModel.TrapStruct.PlacingHeight;
            }
        }

        #endregion
    }
}
