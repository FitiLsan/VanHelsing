namespace BeastHunter
{
    public abstract class BaseInteractiveObjectModel
    {
        #region Properties

        public BaseInteractiveObjectData InteractiveObjectData { get; protected set; }
        public bool IsInteractive { get; set; }

        #endregion
    }
}
