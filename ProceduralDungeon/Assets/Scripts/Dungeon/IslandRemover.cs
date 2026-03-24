using UnityEngine;

namespace ProceduralDungeon
{
    public class IslandRemover : MonoBehaviour, IFinalizer
    {
        public void Finalize(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] != null && metadata[x, y] == null)
                        tiles[x, y] = null;
                }
            }
        }
    }
}