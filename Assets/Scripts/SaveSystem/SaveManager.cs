using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BeastHunter
{
    public sealed class SaveManager : ISaveManager
    {
        #region Fields

        private int _newEntry = 1;
        private readonly ISaveFileWrapper _saveFileWrapper;

        #endregion


        #region Methods

        public SaveManager(ISaveFileWrapper wrapper)
        {
            _saveFileWrapper = wrapper;
        }

        public void SaveGame(string filename)//= null for saving into new file everytime)
        {
            _saveFileWrapper.CreateNewSave(filename ?? DateTime.Now.ToString("s").Replace(':', '-') + ".bytes");
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.Saving, null);
            SaveInfo();
        }

        private void SaveInfo()
        {
            _saveFileWrapper.AddSaveData("NextEntry", _newEntry.ToString());
        }

        public void LoadGame(string filename)
        {
            _saveFileWrapper.LoadSave(filename);
            _newEntry = _saveFileWrapper.GetNextItemEntry();
        }

        #endregion
    }
}