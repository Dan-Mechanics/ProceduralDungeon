using UnityEngine;

namespace ProceduralDungeon
{
    public class RandomWalk : MonoBehaviour, IDungeonGenerator 
    {
        private readonly TileType air;
        private readonly TileType stone;
        private readonly int width;
        private readonly int height;

        private string seed;
        private float stonePercentage;

        public RandomWalk(TileType air, TileType stone, int width, int height)
        {
            this.air = air;
            this.stone = stone;
            this.width = width;
            this.height = height;
        }

        public TileType[,] Generate(Blackboard blackboard)
        {
            seed = blackboard.GetValue<string>(nameof(seed));
            stonePercentage = blackboard.GetValue<float>(nameof(stonePercentage));

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