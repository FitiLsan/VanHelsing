using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class CharacterSave
    {
        #region Fields

        public Race Race;
        public Gender Gender;
        public List<bool> Toggle = new List<bool>();
        public List<SaveItemBody> Items = new List<SaveItemBody>();
        public ColorationSkin Color = new ColorationSkin();

        #endregion

        #region Metods

        public CharacterSave() { }

        public CharacterSave(List<bool> _toggle, List<SaveItemBody> _item, Gender _gender, Race _race, ColorationSkin _color)
        {
            Toggle = _toggle;
            Items = _item;
            Gender = _gender;
            Race = _race;
            Color = _color;
        }

        #endregion
    }
}