namespace BeastHunter
{
    public class TalkingState : CharacterBaseState
    {
        #region Fields


        #endregion


        #region Properties


        #endregion


        #region ClassLifeCycle

        public TalkingState(CharacterModel characterModel, InputModel inputModel, CharacterAnimationController animationController,
            CharacterStateMachine stateMachine) : base(characterModel, inputModel, animationController, stateMachine)
        {
            CanExit = true;
            CanBeOverriden = true;
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

