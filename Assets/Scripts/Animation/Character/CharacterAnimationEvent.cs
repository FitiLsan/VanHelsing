namespace BeastHunter
{
    public sealed class CharacterAnimationEvent
    {
        #region Properties

        public CharacterAnimationEventTypes AnimationEventType { get; }

        #endregion


        #region ClassLifeCycle

        public CharacterAnimationEvent(CharacterAnimationEventTypes type)
        {
            AnimationEventType = type;
        }

        #endregion
    }
}

