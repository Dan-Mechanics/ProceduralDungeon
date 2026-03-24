using UnityEngine;

namespace ProceduralDungeon
{
    public class ContentPlacer : MonoBehaviour, IFinalizer
    {
        [SerializeField] private TileType floor = default;
        [SerializeField] private TileType coins = default;
        [SerializeField] private TileType loot = default;
        [SerializeField] private TileType enemy = default;


        public void Finalize(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
          //  float chance = 0.5f;
            
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] == null)
                        continue;

                    if (Random.value > 0.5f)
                        continue;

                    int neighbours = metadata[x, y].neighbours;
                    switch (neighbours)
                    {
                        case 1:
                            tiles[x, y] = loot;
                            break;
                        case 2:
                            tiles[x, y] = coins;
                            break;
                        case 4:
                            tiles[x, y] = enemy;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}