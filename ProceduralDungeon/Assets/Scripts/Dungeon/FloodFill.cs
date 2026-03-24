using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Flood_fill
    /// https://stackoverflow.com/questions/367226/flood-fill-algorithms
    /// https://medium.com/@akbarnotopb/flood-fill-algorithm-75ac41365639
    /// </summary>
    public class FloodFill : ILayoutAnalyzer
    {
        /// <summary>
        /// Assume that (0, 0) is always filled.
        /// </summary>
        public TileMetadata[,] Analyze(TileType[,] tiles)
        {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            TileMetadata[,] metadata = new TileMetadata[width, height];

            DoFloodFill(0, 0, tiles, metadata, width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (metadata[x, y] != null)
                        metadata[x, y].neighbours = CountNeighbours(x, y, tiles, width, height);
                }
            }

            return metadata;
        }

        /// <summary>
        /// Goal: see if all areas are reachable
        /// on the map and also count the numbers of 
        /// steps to get there, not just the distance.
        /// </summary>
        private void DoFloodFill(int x, int y, TileType[,] tiles, TileMetadata[,] metadata, int width, int height)
        {
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            int stepsTaken = 0;

            stack.Push(new Vector2Int(x, y));
            metadata[x, y] = new TileMetadata(stepsTaken);

            while (stack.Count > 0)
            {
                Vector2Int pos = stack.Pop();
                if (!Utils.Has(pos.x, pos.y, tiles, width, height))
                    continue;

                metadata[pos.x, pos.y] = new TileMetadata(stepsTaken);


                stack.Push(pos + Vector2Int.up);
                stack.Push(pos + Vector2Int.down);
                stack.Push(pos + Vector2Int.left);
                stack.Push(pos + Vector2Int.right);
            }
        }

        private struct Temp
        {
            public int x;
            public int y;
            public int steps;
        }

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