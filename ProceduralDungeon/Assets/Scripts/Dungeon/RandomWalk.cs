using UnityEngine;

namespace ProceduralDungeon
{
    public class RandomWalk : MonoBehaviour, IDungeonGenerator 
    {
        [SerializeField] private TileType stone = default;
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;

        private Vector2Int GetRandomDir()
        {
            int dir = Random.Range(0, 4);
            Vector2Int walkerPos = dir switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.right,
                2 => Vector2Int.left,
                3 => Vector2Int.down,
                _ => throw new System.Exception(),
            };

            return walkerPos;
        }

        public TileType[,] Generate(Blackboard blackboard)
        {
            int iterations = blackboard.GetValue<int>(nameof(iterations));
            int walkLength = blackboard.GetValue<int>(nameof(walkLength));
            bool resetEachWalk = blackboard.GetValue<int>(nameof(resetEachWalk)) == 1;

            string seed = blackboard.GetValue<string>(nameof(seed));
            Random.InitState(seed.GetHashCode());

            TileType[,] tiles = new TileType[width, height];
            Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));
            
            bool backtracking = blackboard.GetValue<int>(nameof(backtracking)) == 1;
            if (backtracking)
            {
                SimpleWalk(tiles, center, iterations, walkLength, resetEachWalk);
            }
            else
            {
                WalkNoBacktracking(tiles, center, iterations, walkLength, resetEachWalk);
            }

            return tiles;
        }

        public void SimpleWalk(TileType[,] tiles, Vector2Int center, int iterations, int walkLength, bool resetEachWalk)
        {
            Vector2Int pos = center;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < walkLength; j++)
                {
                    pos += GetRandomDir();
                    pos.x = Mathf.Clamp(pos.x, 0, width - 1);
                    pos.y = Mathf.Clamp(pos.y, 0, height - 1);

                    tiles[pos.x, pos.y] = stone;
                }

                if (resetEachWalk)
                    pos = center;
            }
        }

        public void WalkNoBacktracking(TileType[,] tiles, Vector2Int center, int iterations, int walkLength, bool resetEachWalk)
        {
            Vector2Int pos = center;
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < walkLength; j++)
                {
                    pos += GetRandomDir();
                    Constrain(ref pos, width, height);
                    while (tiles[pos.x, pos.y] != null)
                    {
                        pos += GetRandomDir();
                        Constrain(ref pos, width, height);
                    }

                    pos.x = Mathf.Clamp(pos.x, 0, width - 1);
                    pos.y = Mathf.Clamp(pos.y, 0, height - 1);

                    tiles[pos.x, pos.y] = stone;
                }

                if (resetEachWalk)
                    pos = center;
            }
        }

        private void Constrain(ref Vector2Int vec, int w, int h)
        {
            vec.x = Mathf.Clamp(vec.x, 0, w - 1);
            vec.y = Mathf.Clamp(vec.y, 0, h - 1);
        }
    }
}