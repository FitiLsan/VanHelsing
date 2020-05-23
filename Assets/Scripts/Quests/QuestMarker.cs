using UnityEngine;


namespace BeastHunter
{
    public sealed class QuestMarker
    {
        #region Properties

        public int MapId { get; }
        public Vector2 Position { get; }

        #endregion


        #region ClassLifeCycle

        public QuestMarker(int mapId, float x, float y)
        {
            MapId = mapId;
            Position = new Vector2(x, y);
        }

        public QuestMarker(QuestMarkerDto dto)
        {
            MapId = dto.MapId;
            Position = new Vector2(dto.X, dto.Y);
        }

        #endregion
    }
}