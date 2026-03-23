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
            // todo: make this work, use resources in summary.

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // we need to use recursion somehow.. lovely.
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (metadata[x, y] != null)
                        metadata[x, y].neighbours = CountNeighboursDiag(x, y, tiles, width, height);
                }
            }

            return metadata;
        }

        private int CountNeighboursDiag(int x, int y, TileType[,] tiles, int width, int height)
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