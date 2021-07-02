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

        public int InstanceID { get; private set; }
        public int HubMapIndex { get; private set; }
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

        #endregion


        #region ClassLifeCycle

        public MapObjectModel(MapObjectData mapObjectData)
        {
            InstanceID = mapObjectData.InstanceId;
            HubMapIndex = mapObjectData.HubMapIndex;
            Name = mapObjectData.Name;
            Description = mapObjectData.Description;
            IsBlocked = mapObjectData.IsBlocked;
        }

        #endregion
    }
}
