using System;

namespace Events.Args
{
    /// <summary>
    ///     Параметры с Id
    /// </summary>
    public class IdArgs : EventArgs
    {
        public IdArgs(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}