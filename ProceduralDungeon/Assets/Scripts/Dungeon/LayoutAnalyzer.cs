using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    public class LayoutAnalyzer : MonoBehaviour
    {
        private Dictionary<Vector2Int, TileType> tiles;

        /// <summary>
        /// https://stackoverflow.com/questions/367226/flood-fill-algorithms
        /// Make something called remove islands for this bitch.
        /// </summary>
        public Dictionary<Vector2Int, TileMetadata> Analyze(Dictionary<Vector2Int, TileType> tiles, Blackboard blackboard) 
        {
            this.tiles = tiles;

            Dictionary<Vector2Int, TileMetadata> metadata = new Dictionary<Vector2Int, TileMetadata>();
            foreach (var item in tiles)
            {
                metadata.Add(item.Key, new TileMetadata());
            }


            Queue<Vector2Int> pending = new Queue<Vector2Int>();
            HashSet<Vector2Int> processed = new HashSet<Vector2Int>();

            pending.Enqueue(Vector2Int.zero);

            /*while (pending.Count > 0)
            {
                Vector2Int pos = pending.Dequeue();
                metadata[pos].neighbourCount = GetNeighbourCount(pos);
                metadata[pos].roomsFromStart()
                processed.Add(pos);

                Spread(pos + Vector2Int.up, processed, pending);
                Spread(pos + Vector2Int.down, processed, pending);
                Spread(pos + Vector2Int.left, processed, pending);
                Spread(pos + Vector2Int.right, processed, pending);
            }*/

            // flood fill to find distance from start.
            // deadend calc.
            HashSet<Vector2Int> sprouts = new HashSet<Vector2Int>();
            sprouts.Add(Vector2Int.zero);
            while (sprouts.Count > 0)
            {
                Vector2Int first = sprouts.First();

            }

            return metadata;
        }

        private void Spread(Vector2Int pos, HashSet<Vector2Int> processed, Queue<Vector2Int> pending)
        {
            // IF THIS TILE DOESNT EXIST OR WE ALREADY CALCULATED THE VALUE.
            if (processed.Contains(pos) || !Has(pos))
                return;

            pending.Enqueue(pos);
        }

        private bool Has(Vector2Int pos) => tiles.ContainsKey(pos);

        private int GetNeighbourCount(Vector2Int pos)
        {
            int count = 0;
            if (Has(pos + Vector2Int.up))
                count++;

            if (Has(pos + Vector2Int.down))
                count++;

            if (Has(pos + Vector2Int.left))
                count++;

            if (Has(pos + Vector2Int.right))
                count++;

            return count;
        }

        public struct TileMetadata
        {
            public int roomsFromStart;
            public int neighbourCount;
        }
    }
}