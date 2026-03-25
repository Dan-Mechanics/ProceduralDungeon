using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [CreateAssetMenu(fileName = nameof(Room), menuName = nameof(Room))]
    public class Room : ScriptableObject
    {
        public Texture2D texture;
        public bool terminateWalkAfter;
        public TileType floor;
        public TileType remove;

        public void Apply(TileType[,] tiles, int xPos, int yPos, Vector2Int center, Dictionary<Color, TileType> colorToType)
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
                    TileType type = stamp[x, y];
                    ApplyTile(x + xPos - halfX, y + yPos - halfY,
                        width, height, center, type, tiles);
                }
            }
        }

        private void ApplyTile(int x, int y, int w, int h, Vector2Int center, TileType type, TileType[,] tiles)
        {
            if (x < 0 || x >= w || y < 0 || y >= h)
                return;

            if (x == center.x && y == center.y)
                return;

            if (tiles[x, y] == null || tiles[x, y] == floor)
                tiles[x, y] = type;

            if (tiles[x, y] == remove)
                tiles[x, y] = null;
        }

        private TileType[,] GetStamp(Dictionary<Color, TileType> colorToType)
        {
            if (!texture.isReadable)
                throw new System.Exception("if (!texture.isReadable)");
            
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

        private void OnValidate() => texture = Resources.Load<Texture2D>(name);
    }
}