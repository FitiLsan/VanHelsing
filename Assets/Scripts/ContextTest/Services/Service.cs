namespace BeastHunter
{
    public abstract class Service
    {
        #region Fields

        protected readonly Contexts _contexts;

        #endregion


        #region ClassLifeCycles

        protected Service(Contexts contexts)
        {
            _contexts = contexts;
        }

        #endregion
    }
}
