using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Flood_fill
    /// https://stackoverflow.com/questions/367226/flood-fill-algorithms
    /// https://medium.com/@akbarnotopb/flood-fill-algorithm-75ac41365639
    /// </summary>
    public class FloodFill : MonoBehaviour, ILayoutAnalyzer
    {
        public TileMetadata[,] Analyze(TileType[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            TileMetadata[,] metadata = new TileMetadata[width, height];

            Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));
            DoFloodFill(center, tiles, metadata, width, height);

            HashSet<Vector2Int> walkable = RemoveIslands(tiles, width, height, metadata);
            CountSteps(center, walkable, metadata);

            return metadata;
        }

        private HashSet<Vector2Int> RemoveIslands(TileType[,] tiles, int width, int height, TileMetadata[,] metadata)
        {
            HashSet<Vector2Int> walkable = new HashSet<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] != null && metadata[x, y] == null)
                        tiles[x, y] = null;

                    if (metadata[x, y] != null)
                        walkable.Add(new Vector2Int(x, y));
                }
            }

            return walkable;
        }

        private void DoFloodFill(Vector2Int center, TileType[,] tiles, TileMetadata[,] metadata, int width, int height)
        {
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(center);

            while (stack.Count > 0)
            {
                Vector2Int pos = stack.Pop();
                if (!CanFillPos(tiles, metadata, width, height, pos))
                    continue;

                metadata[pos.x, pos.y] = new TileMetadata(pos.x, pos.y,
                        CountNeighbours(pos.x, pos.y, tiles, width, height));

                stack.Push(pos + Vector2Int.up);
                stack.Push(pos + Vector2Int.down);
                stack.Push(pos + Vector2Int.left);
                stack.Push(pos + Vector2Int.right);
            }
        }

        private bool CanFillPos(TileType[,] tiles, TileMetadata[,] metadata, int width, int height, Vector2Int pos) => 
            Utils.Has(pos.x, pos.y, tiles, width, height) && !Utils.Has(pos.x, pos.y, metadata, width, height);

        private void CountSteps(Vector2Int center, HashSet<Vector2Int> walkable, TileMetadata[,] metadata)
        {
            Queue<Vector2Int> sprouts = new Queue<Vector2Int>();
            List<Vector2Int> newSprouts = new List<Vector2Int>();
            int steps = 0;

            sprouts.Enqueue(center);
            metadata[center.x, center.y].steps = 0;
            while (sprouts.Count > 0)
            {
                while (sprouts.Count > 0)
                {
                    Vector2Int pos = sprouts.Dequeue();
                    if (!walkable.Contains(pos))
                        continue;

                    metadata[pos.x, pos.y].steps = steps;
                    walkable.Remove(pos);

                    newSprouts.Add(pos + Vector2Int.up);
                    newSprouts.Add(pos + Vector2Int.down);
                    newSprouts.Add(pos + Vector2Int.left);
                    newSprouts.Add(pos + Vector2Int.right);
                }

                newSprouts.ForEach(x => sprouts.Enqueue(x));
                newSprouts.Clear();

                steps++;
            }
        }

        /// <summary>
        /// Diagonally.
        /// </summary>
        private int CountNeighbours(int x, int y, TileType[,] tiles, int width, int height)
        {
            int count = 0;
            if (Utils.Has(x + 1, y, tiles, width, height))
                count++;

            if (Utils.Has(x - 1, y, tiles, width, height))
                count++;

            if (Utils.Has(x, y + 1, tiles, width, height))
                count++;

            if (Utils.Has(x, y - 1, tiles, width, height))
                count++;

            return count;
        }
    }
}