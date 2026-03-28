using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// Counting loot and coins as objectives.
    /// </summary>
    public class ContentPlacer : MonoBehaviour, IContentPlacer
    {
        [SerializeField] private TileType floor = default;
        [SerializeField] private TileType coins = default;
        [SerializeField] private TileType loot = default;
        [SerializeField] private TileType start = default;
        [SerializeField] private TileType goal = default;
        [SerializeField] private TileType lava = default;

        public void PlaceContent(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
            float coinOdds = blackboard.GetValue<float>(nameof(coinOdds));
            float lavaOdds = blackboard.GetValue<float>(nameof(lavaOdds));
            float lootOdds = blackboard.GetValue<float>(nameof(lootOdds));

            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            List<TileMetadata> walkable = new List<TileMetadata>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] != floor)
                        continue;

                    walkable.Add(metadata[x, y]);
                    switch (metadata[x, y].neighbours)
                    {
                        // 1 NEIGHBOUR = 3 WALLS AROUND.
                        case 1:
                            if (Utils.Roll(lootOdds))
                                tiles[x, y] = loot;

                            break;
                        // 2 NEIGHBOURS = 2 WALLS AROUND.
                        case 2:
                            if (Utils.Roll(lavaOdds) && IsHallway(x, y, metadata, width, height))
                            {
                                tiles[x, y] = lava;
                            }
                            else if (Utils.Roll(coinOdds)) 
                            {
                                tiles[x, y] = coins;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            // SORT, PLACE START AND END GOAL.
            walkable.Sort((x, y) => x.steps.CompareTo(y.steps));
            if (walkable.Count > 0)
                Place(walkable[0], start, tiles);

            if (walkable.Count > 1)
                Place(walkable[^1], goal, tiles);
        }

        /// <summary>
        /// Assuming that (x, y) is already a hallway.
        /// </summary>
        private bool IsHallway(int x, int y, TileMetadata[,] metadata, int width, int height)
        {
            if (CheckNeighbours(x + 1, y, metadata, 2, width, height) && CheckNeighbours(x - 1, y, metadata, 2, width, height))
                return true;

            if (CheckNeighbours(x, y + 1, metadata, 2, width, height) && CheckNeighbours(x, y - 1, metadata, 2, width, height))
                return true;

            return false;
        }

        private bool CheckNeighbours(int x, int y, TileMetadata[,] metadata, int count, int width, int height)
            => Utils.Has(x, y, metadata, width, height) && metadata[x, y].neighbours == count;

        private void Place(TileMetadata pos, TileType type, TileType[,] tiles) 
            => tiles[pos.x, pos.y] = type;
    }
}