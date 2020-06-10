using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class ItemBody
    {
        #region Fields

        public GameObject Object;
        public int Position;
        public bool Visible;

        #endregion

        #region Metods

        public ItemBody() { }

        public ItemBody(GameObject _obj, int _position, bool _visible)
        {
            Object = _obj;
            Position = _position;
            Visible = _visible;
        }

        public void Clear()
        {
            Object = null;
            Position = 0;
        }

        #endregion
    }
}
