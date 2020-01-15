using UnityEngine;

namespace Quests
{
    /// <summary>
    ///     информация об отметках на карте
    /// </summary>
    public class QuestMarker
    {
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

        /// <summary>
        ///     ИД карты
        /// </summary>
        public int MapId { get; }

        /// <summary>
        ///     Позиция метки
        /// </summary>
        public Vector2 Position { get; }
    }
}