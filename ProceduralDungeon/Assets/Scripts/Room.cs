using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [CreateAssetMenu(fileName = nameof(Room), menuName = nameof(Room))]
    public class Room : ScriptableObject
    {
        [Tooltip("Make sure to enable Read/Write on this texture.")]
        public Texture2D texture;
        public Conversion[] conversions;

        private bool hasSetup;
        private TileType[,] stamp;
        private Dictionary<Color, TileType> colorToType;

        public void Apply(TileType[,] tiles, int xPos, int yPos)
        {
            if (!hasSetup)
            {
                colorToType = new Dictionary<Color, TileType>();
                for (int i = 0; i < conversions.Length; i++)
                {
                    colorToType.Add(conversions[i].color, conversions[i].type);
                }

                stamp = GetStamp();
                Debug.Log("Setup done.");
                hasSetup = true;
            }

            int w = stamp.GetLength(0);
            int h = stamp.GetLength(1);
            int halfX = Mathf.FloorToInt(w / 2f);
            int halfY = Mathf.FloorToInt(h / 2f);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    TryApply(x + xPos - halfX, y + yPos - halfY, tiles, stamp[x, y]);
                }
            }
        }

        public void TryApply(int x, int y, TileType[,] tiles, TileType type)
        {
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
                return;
            
            tiles[x, y] = type;
        }

        private TileType[,] GetStamp()
        {
            int w = texture.width;
            int h = texture.height;
            TileType[,] tiles = new TileType[texture.width, texture.height];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color color = texture.GetPixel(x, y);
                    if (colorToType.ContainsKey(color))
                        tiles[x, y] = colorToType[color];
                }
            }

            return tiles;
        }

        private void OnValidate()
        {
            hasSetup = false;
            for (int i = 0; i < conversions.Length; i++)
            {
                conversions[i].color.a = 1f;
            }
        }

        [System.Serializable]
        public struct Conversion
        {
            public Color color;
            public TileType type;
        }
    }
}
