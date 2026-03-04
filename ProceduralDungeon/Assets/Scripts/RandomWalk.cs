using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class RandomWalk : ILayoutGenerator 
    {
        public int seed;
        public TileType air;
        public TileType stone;
        [Min(1)] public int width;
        [Min(1)] public int height;
        public int tileWidth;
        public int tileHeight;
        [Range(0f, 1f)] public float stonePercentage;

        public RandomWalk(int seed, TileType air, TileType stone, int width, int height, int tileWidth, int tileHeight, float stonePercentage)
        {
            this.seed = seed;
            this.air = air;
            this.stone = stone;
            this.width = width;
            this.height = height;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.stonePercentage = stonePercentage;
        }

        public Tile[,] Generate()
        {
            Random.InitState(seed);
            
            Tile[,] tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile temp = tiles[x, y];
                    temp.x = x;
                    temp.y = y;
                    temp.width = tileWidth;
                    temp.height = tileHeight;
                    temp.type = Random.value < stonePercentage ? stone : air;
                    tiles[x, y] = temp;
                }
            }

            return tiles;
        }
    }
}