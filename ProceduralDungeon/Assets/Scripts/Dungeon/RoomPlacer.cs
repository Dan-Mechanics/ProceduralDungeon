using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class RoomPlacer : MonoBehaviour, IContentPlacer
    {
        [SerializeField] private Texture2D colorIndex = default;
        [SerializeField] private TileType[] types = default;
        [SerializeField] private Room[] rooms = default;
        private Dictionary<Color, TileType> colorToType;

        private Dictionary<Color, TileType> GetColorToType()
        {
            Dictionary<Color, TileType> colorToType = new Dictionary<Color, TileType>();
            for (int i = 0; i < types.Length; i++)
            {
                colorToType.Add(colorIndex.GetPixel(0, i), types[i]);
            }

            return colorToType;
        }

        public void PlaceContent(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
            if (colorToType == null)
                colorToType = GetColorToType();
            
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (metadata[x, y] == null)
                        continue;

                    if (metadata[x, y].neighbours != 1)
                        continue;

                    if (Random.value > 0.2)
                        continue;

                 //   rooms[Random.Range(0, rooms.Length)].Apply(tiles, x, y, colorToType);
                    // place a room if certain oods?
                }
            }
        }
    }
}