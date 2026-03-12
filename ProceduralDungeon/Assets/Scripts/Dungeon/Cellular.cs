using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://www.youtube.com/watch?v=stgYW6M5o4k
    /// </summary>
    public class Cellular : MonoBehaviour, IDungeonGenerator 
    {
        public enum Faction { Red, Green, Blue }
        
        [SerializeField] private TileType stone = default;
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;

        private Faction GetRandomFaction() => (Faction)Random.Range(0, 3);

        private Faction GetFood(Faction attacker)
        {
            return attacker switch
            {
                Faction.Red => Faction.Green,
                Faction.Green => Faction.Blue,
                Faction.Blue => Faction.Red,
                _ => throw new System.Exception(),
            };
        }

        public TileType[,] Generate(Blackboard blackboard)
        {
            int iterations = blackboard.GetValue<int>(nameof(iterations));
            int backtracking = blackboard.GetValue<int>(nameof(backtracking));

            string seed = blackboard.GetValue<string>(nameof(seed));
            Random.InitState(seed.GetHashCode());

            Faction[,] readBuffer = new Faction[width, height];
            Faction[,] writeBuffer = new Faction[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    readBuffer[x, y] = GetRandomFaction();
                    writeBuffer[x, y] = readBuffer[x, y];
                }
            }

            Faction attacker = Faction.Red;
            Faction food = GetFood(attacker);
            for (int i = 0; i < iterations; i++)
            {
                Simulate(attacker, food, readBuffer, writeBuffer);
                attacker = food;
                food = GetFood(attacker);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        readBuffer[x, y] = writeBuffer[x, y];
                    }
                }
            }

            Faction screenshot = (Faction)backtracking;
            TileType[,] tiles = new TileType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    /*switch (readBuffer[x, y])
                    {
                        case Faction.Red:
                            tiles[x, y] = red;
                            break;
                        case Faction.Green:
                            tiles[x, y] = green;
                            break;
                        case Faction.Blue:
                            tiles[x, y] = blue;
                            break;
                        default:
                            break;
                    }*/
                    if (readBuffer[x, y] == screenshot)
                        tiles[x, y] = stone;
                }
            }

            return tiles;
        }

        private void Simulate(Faction attacker, Faction food, Faction[,] readBuffer, Faction[,] writeBuffer)
        {
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (readBuffer[x, y] == attacker)
                        Spread(x, y, attacker, food, readBuffer, writeBuffer);
                }
            }
        }

        private void Spread(int parentX, int parentY, Faction attacker, Faction food, Faction[,] readBuffer, Faction[,] writeBuffer)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    /*if (x == y)
                        continue;*/

                    // THIS WORKS WELL:
                    if (Random.value > 0.5)
                        continue;

                    int newX = x + parentX;
                    int newY = y + parentY;
                    if (readBuffer[newX, newY] == food)
                        writeBuffer[newX, newY] = attacker;
                }
            }
        }
    }
}