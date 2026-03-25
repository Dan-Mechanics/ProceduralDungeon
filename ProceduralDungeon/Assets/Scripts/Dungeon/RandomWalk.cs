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

        private float sameDirectionOdds;
        private int iterations;
        private float maxDistance;
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

            Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));

            sameDirectionOdds = 0.75f;
            iterations = 80;
            maxDistance = 112f;

            // TODO: MAKE THESE ALL BLACKBOARD VALUES !!
            TileType[,] tiles = new TileType[width, height];
            tiles[center.x, center.y] = floor;

            SendWalker(tiles, center, Vector2Int.right);
            SendWalker(tiles, center, Vector2Int.up);
            SendWalker(tiles, center, Vector2Int.down);
            SendWalker(tiles, center, Vector2Int.left);
            return tiles;
        }

        private void SendWalker(TileType[,] tiles, Vector2Int startingPosition,  Vector2Int startingDirection) 
        {
            Vector2Int pos = startingPosition;
            Vector2Int dir = startingDirection;
            for (int i = 0; i < iterations; i++)
            {
                if (Random.value > sameDirectionOdds)
                    dir = GetRandomDir();

                while (Utils.Has(pos.x, pos.y, tiles, width, height))
                {
                    pos += dir;
                }

                if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
                    return;

                tiles[pos.x, pos.y] = floor;
                if (Vector2Int.Distance(pos, Vector2Int.zero) > maxDistance)
                    return;
            }
        }
    }
}