using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Random_walk
    /// </summary>
    public class RandomWalk : MonoBehaviour, ILayoutGenerator 
    {
        [SerializeField] private TileType floor = default;
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;
        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.down
        };

        private Vector2Int GetRandomDir() => directions[Random.Range(0, directions.Length)];

        public TileType[,] Generate(Blackboard blackboard)
        {
            string seed = blackboard.GetValue<string>(nameof(seed));
            if (!Utils.IsStringValid(seed))
                seed = "default";

            Random.InitState(seed.GetHashCode());

            // TODO: MAKE THESE ALL BLACKBOARD VALUES !!
            TileType[,] tiles = new TileType[width, height];
            SendWalker(tiles, Vector2Int.right, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.up, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.down, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.left, 0.5f, 80, 112f);
            return tiles;
        }

        private void SendWalker(TileType[,] tiles, Vector2Int startingDirection, float sameDirectionOdds, int iterations, float maxDistance)
        {
            Vector2Int pos = Vector2Int.zero;
            Vector2Int dir = startingDirection;
            for (int i = 0; i < iterations; i++)
            {
                if (Random.value < sameDirectionOdds)
                    dir = GetRandomDir();

                pos += dir;
                while (Utils.Has(pos.x, pos.y, tiles, width, height))
                {
                    dir = GetRandomDir();
                    pos += dir;
                }

                tiles[pos.x, pos.y] = floor;
                if (Vector2Int.Distance(pos, Vector2Int.zero) > maxDistance)
                    return;
            }
        }
    }
}