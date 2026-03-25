using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class ContentPlacer : MonoBehaviour, IContentPlacer
    {
        [SerializeField] private TileType floor = default;
        [SerializeField] private TileType coins = default;
        [SerializeField] private TileType loot = default;
        [SerializeField] private TileType start = default;
        [SerializeField] private TileType goal = default;

        public void PlaceContent(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
            float coinOdds = 0.2f;
            
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
                        case 1:
                            tiles[x, y] = loot;
                            break;
                        case 2:
                            if (Random.value < coinOdds)
                                tiles[x, y] = coins;

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

        private void Place(TileMetadata pos, TileType type, TileType[,] tiles) => tiles[pos.x, pos.y] = type;
    }
}