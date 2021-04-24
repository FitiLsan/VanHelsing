using System;


namespace BeastHunterHubUI
{
    public abstract class MapObjectModel
    {
        #region Fields

        private bool _isBlocked;

        #endregion


        #region Properties

        public Action<MapObjectModel> OnChangeBlockedStatus { get; set; }

        public MapObjectType MapObjectType { get; private set; }
        public int DataInstanceID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public MapObjectBehaviour Behaviour { get; set; }

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

        public MapObjectModel(MapObjectData mapObjectData)
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
