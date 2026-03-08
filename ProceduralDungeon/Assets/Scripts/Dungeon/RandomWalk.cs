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
            float stonePercentage = blackboard.GetValue<float>(nameof(stonePercentage));

            // ===

            Random.InitState(seed.GetHashCode());

            TileType[,] tiles = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = Random.value < stonePercentage ? stone : air;
                }
            }

            return tiles;
        }
    }
}