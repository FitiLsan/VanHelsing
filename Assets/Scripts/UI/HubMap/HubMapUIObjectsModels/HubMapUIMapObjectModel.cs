using System;


namespace BeastHunter
{
    public abstract class HubMapUIMapObjectModel
    {
        #region Fields

        private bool _isBlocked;

        #endregion


        #region Properties

        public Action<HubMapUIMapObjectModel> OnChangeBlockedStatus { get; set; }

        public HubMapUIMapObjectType MapObjectType { get; private set; }
        public int DataInstanceID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public HubMapUIMapObjectBehaviour Behaviour { get; set; }

        public bool IsBlocked
        {
            get
            {
                return _isBlocked;
            }
            set
            {
                if (value != _isBlocked)
                {
                    _isBlocked = value;
                    OnChangeBlockedStatus?.Invoke(this);
                }
            }
        }

        public HubMapUIMapObjectModel(HubMapUIMapObjectData mapObjectData)
        {
            MapObjectType = mapObjectData.GetMapObjectType();
            DataInstanceID = mapObjectData.GetInstanceID();
            Name = mapObjectData.Name;
            Description = mapObjectData.Description;
            IsBlocked = mapObjectData.IsBlockedAtStart;
        }

        #endregion
    }
}
