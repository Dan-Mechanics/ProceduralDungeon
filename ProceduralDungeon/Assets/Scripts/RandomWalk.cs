using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class RandomWalk : ILayoutGenerator 
    {
        public string seed;
        public TileType air;
        public TileType stone;
        [Min(1)] public int width;
        [Min(1)] public int height;
        public int tileWidth;
        public int tileHeight;
        [Range(0f, 1f)] public float stonePercentage;

        public void OnLoad(Blackboard blackboard)
        {
            //seed = blackboard.GetValue<string>(nameof(seed));
            File.WriteAllText("", blackboard.ToString());
            int kaas = blackboard.GetValue<int>(nameof(kaas));
        }

        public Tile[,] Generate()
        {
            Random.InitState(seed.GetHashCode());

            OnLoad(new Blackboard());

            // f, i, b, s
            object test = 10f;
            char c = ' ';
            if (test is float)
            {
                c = 'f';
            }
            else if(test is int)
            {
                c = 'i';
            }
            else if(test is string)
            {
                c = 's';
            }
            else if(test is bool)
            {
                c = 'b';
            }

            Debug.Log($"{c},{nameof(test)},{test}");

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