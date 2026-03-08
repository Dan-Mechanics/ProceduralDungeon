using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://www.youtube.com/watch?v=stgYW6M5o4k
    /// </summary>
    public class Cellular : MonoBehaviour, IDungeonGenerator 
    {
        [SerializeField] private TileType red = default;
        [SerializeField] private TileType green = default;
        [SerializeField] private TileType blue = default;
        [SerializeField] private TileType stone = default;
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;

        private TileType GetRandomFaction()
        {
            int faction = Random.Range(0, 3);
            switch (faction)
            {
                case 0:
                    return red;
                case 1:
                    return green;
                case 2:
                    return blue;
                default:
                    throw new System.Exception();
            }
        }

        public TileType[,] Generate(Blackboard blackboard)
        {
            string seed = blackboard.GetValue<string>(nameof(seed));
          // int iterations = blackboard.GetValue<int>(nameof(iterations));
          // int walkLength = blackboard.GetValue<int>(nameof(walkLength));
          // bool resetEachWalk = blackboard.GetValue<int>(nameof(resetEachWalk)) == 1;
          // bool backtracking = blackboard.GetValue<int>(nameof(backtracking)) == 1;

            Random.InitState(seed.GetHashCode());
            TileType[,] tiles = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = GetRandomFaction();
                    if(tiles[x,y] == red)
                    {
                        tiles[x, y] = stone;
                    }
                    else
                    {
                        tiles[x, y] = null;
                    }
                }
            }

           /* Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));
            if (backtracking)
            {
                SimpleWalk(tiles, center, iterations, walkLength, resetEachWalk);
            }
            else
            {
                WalkNoBacktracking(tiles, center, iterations, walkLength, resetEachWalk);
            }*/

            return tiles;
        }
    }
}