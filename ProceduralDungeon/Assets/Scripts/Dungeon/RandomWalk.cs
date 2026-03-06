using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// doesnt need to be mono anymore/
    /// </summary>
    public class RandomWalk : MonoBehaviour, IDungeonGenerator 
    {
        public TileType[,] Generate(Blackboard blackboard)
        {
            int width = blackboard.GetValue<int>(nameof(width));
            int height = blackboard.GetValue<int>(nameof(height));

            string seed = blackboard.GetValue<string>(nameof(seed));
            float stonePercentage = blackboard.GetValue<float>(nameof(stonePercentage));

            TileType air = blackboard.GetValue<TileType>(nameof(air));
            TileType stone = blackboard.GetValue<TileType>(nameof(stone));

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