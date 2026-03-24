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

            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));

            sameDirectionOdds = 0.5f;
            iterations = 80;
            maxDistance = 112f;

            // TODO: MAKE THESE ALL BLACKBOARD VALUES !!
            TileType[,] tiles = new TileType[width, height];
            SendWalker(tiles, pos, Vector2Int.right);
            SendWalker(tiles, pos, Vector2Int.up);
            SendWalker(tiles, pos, Vector2Int.down);
            SendWalker(tiles, pos, Vector2Int.left);
            return tiles;
        }

        private void SendWalker(TileType[,] tiles, Vector2Int startingPosition,  Vector2Int startingDirection) {
            Vector2Int pos = startingPosition;
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