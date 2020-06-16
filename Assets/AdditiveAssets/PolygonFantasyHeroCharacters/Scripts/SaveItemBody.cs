using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public class SaveItemBody
    {
        #region Fields

        public BodyItems Body;
        public int Position;
        public bool Visible;

        #endregion

        #region Metods
        public SaveItemBody() { }

        public SaveItemBody(BodyItems _body, ItemBody _item)
        {
            Body = _body;
            Position = _item.Position;
            Visible = _item.Visible;
        }
        #endregion
    }
}
