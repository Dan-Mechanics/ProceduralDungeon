using UnityEngine;
namespace ProceduralDungeon
{
    /// <summary>
    /// https://www.youtube.com/watch?v=stgYW6M5o4k
    /// </summary>
    public class RandomWalk : MonoBehaviour, IDungeonGenerator 
    {
        [SerializeField] private TileType air = default;
        [SerializeField] private TileType stone = default;

        // YOU COULD MAKE THESE BLACKBOARD IN THE FUTURE.
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;

        public TileType[,] Generate(Blackboard blackboard)
        {
            string seed = blackboard.GetValue<string>(nameof(seed));
            int iterations = blackboard.GetValue<int>(nameof(iterations));
           // float stonePercentage = blackboard.GetValue<float>(nameof(stonePercentage));

            // ===

            Random.InitState(seed.GetHashCode());

            TileType[,] tiles = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = stone;
                }
            }

            // SIMPLE WALKER, NORMALIZED.
            Vector2Int walkerPos = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));
            for (int i = 0; i < iterations; i++)
            {
                int dir = Random.Range(0, 4);
                walkerPos += dir switch
                {
                    0 => Vector2Int.up,
                    1 => Vector2Int.right,
                    2 => Vector2Int.left,
                    3 => Vector2Int.down,
                    _ => throw new System.Exception(),
                };

                walkerPos.x = Mathf.Clamp(walkerPos.x, 0, width - 1);
                walkerPos.y = Mathf.Clamp(walkerPos.y, 0, height - 1);
                tiles[walkerPos.x, walkerPos.y] = air;
            }

            return tiles;
        }
    }
}