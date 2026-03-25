using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [CreateAssetMenu(fileName = nameof(Room), menuName = nameof(Room))]
    public class Room : ScriptableObject
    {
        [Tooltip("Make sure to enable Read/Write on this texture.")]
        public bool terminateAfterRoom;
        public Texture2D texture;
        public TileType floor;
        public TileType remove;

        public void Apply(TileType[,] tiles, int xPos, int yPos, Dictionary<Color, TileType> colorToType)
        {
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

        private void ApplyTile(int x, int y, TileType[,] tiles, TileType type)
        {
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
                return;
            
            if (tiles[x,y] == null || tiles[x,y] == floor)
                tiles[x, y] = type;

            if (tiles[x, y] == remove)
                tiles[x, y] = null;
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
    }
}