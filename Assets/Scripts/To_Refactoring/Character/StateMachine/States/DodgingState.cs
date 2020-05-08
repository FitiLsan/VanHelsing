namespace BeastHunter
{
    public class DodgingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public DodgingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = false;
            CanBeOverriden = false;
        }

        #endregion


        #region Methods

        public override void Initialize()
        {

        }

        public override void Execute()
        {

        }

        public override void OnExit()
        {

        }

        #endregion
    }
}

