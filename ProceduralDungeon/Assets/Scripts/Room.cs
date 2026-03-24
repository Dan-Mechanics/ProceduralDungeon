using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// I think it might be smart to put rooms
    /// in script itself instead of here, then we just need references to shit type beat.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(Room), menuName = nameof(Room))]
    public class Room : ScriptableObject
    {
        [Tooltip("Make sure to enable Read/Write on this texture.")]
        public Texture2D texture;
        public Conversion[] conversions;

        public void Apply(TileType[,] tiles, int xPos, int yPos)
        {
            Dictionary<Color, TileType> colorToType = new Dictionary<Color, TileType>();
            for (int i = 0; i < conversions.Length; i++)
            {
                colorToType.Add(conversions[i].color, conversions[i].type);
            }

            TileType[,] stamp = GetStamp(colorToType);

            int width = stamp.GetLength(0);
            int height = stamp.GetLength(1);
            int halfX = Mathf.FloorToInt(width / 2f);
            int halfY = Mathf.FloorToInt(height / 2f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ApplyTile(x + xPos - halfX, y + yPos - halfY, tiles, stamp[x, y]);
                }
            }
        }

        public void ApplyTile(int x, int y, TileType[,] tiles, TileType type)
        {
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
                return;
            
            tiles[x, y] = type;
        }

        private TileType[,] GetStamp(Dictionary<Color, TileType> colorToType)
        {
            int width = texture.width;
            int height = texture.height;
            TileType[,] tiles = new TileType[texture.width, texture.height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = texture.GetPixel(x, y);
                    if (colorToType.ContainsKey(color))
                        tiles[x, y] = colorToType[color];
                }
            }

            return tiles;
        }

        [System.Serializable]
        public struct Conversion
        {
            public Color color;
            public TileType type;
        }
    }
}
