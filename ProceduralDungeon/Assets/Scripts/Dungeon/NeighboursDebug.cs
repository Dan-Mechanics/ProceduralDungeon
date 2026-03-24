using UnityEngine;

namespace ProceduralDungeon
{
    public class NeighboursDebug : MonoBehaviour, IContentPlacer
    {
        [SerializeField] private TileType[] neighbours = default;

        public void PlaceContent(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] != null)
                        tiles[x, y] = neighbours[metadata[x, y].neighbours];
                }
            }
        }
    }
}