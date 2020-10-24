using System.Collections.Generic;


namespace BeastHunter
{
    public interface ISaveManager
    {
        #region Methods

        void SaveGame(string file);
        void LoadGame(string file);

        #endregion
    }
}