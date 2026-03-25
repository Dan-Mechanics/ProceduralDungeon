using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Random_walk
    /// </summary>
    public class RandomWalkWithRooms : MonoBehaviour, ILayoutGenerator 
    {
        [SerializeField] private TileType floor = default;
        [SerializeField] private int width = default;
        [SerializeField] private int height = default;
        [SerializeField] private Texture2D colorIndex = default;
        [SerializeField] private TileType[] types = default;
        [SerializeField] private Room[] rooms = default;
        private Dictionary<Color, TileType> colorToType;

        private float sameDirectionOdds;
        private float maxDistance;
        private int iterations;
        private float roomOdds;
        private float tilesForRoomChance;

        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.down
        };

        private Dictionary<Color, TileType> GetColorToType()
        {
            Dictionary<Color, TileType> colorToType = new Dictionary<Color, TileType>();
            for (int i = 0; i < types.Length; i++)
            {
                colorToType.Add(colorIndex.GetPixel(0, i), types[i]);
            }

            return colorToType;
        }

        private Vector2Int GetRandomDir() => directions[Random.Range(0, directions.Length)];

        public TileType[,] Generate(Blackboard blackboard)
        {
            if (colorToType == null)
                colorToType = GetColorToType();

            string seed = blackboard.GetValue<string>(nameof(seed));
            if (!Utils.IsStringValid(seed))
                seed = "default";

            Random.InitState(seed.GetHashCode());

            Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));

            sameDirectionOdds = 0.3f;
            iterations = 80;
            maxDistance = 112f;
            tilesForRoomChance = 10;
            roomOdds = 0.5f;

            // TODO: MAKE THESE ALL BLACKBOARD VALUES !!
            TileType[,] tiles = new TileType[width, height];
            tiles[center.x, center.y] = floor;

            SendWalker(tiles, center, Vector2Int.right);
            SendWalker(tiles, center, Vector2Int.up);
            SendWalker(tiles, center, Vector2Int.down);
            SendWalker(tiles, center, Vector2Int.left);
            return tiles;
        }

        private void SendWalker(TileType[,] tiles, Vector2Int center, Vector2Int primaryDirection) 
        {
            Vector2Int pos = center;
            Vector2Int dir;

            int tileCounter = 0;
            for (int i = 0; i < iterations; i++)
            {
                // DO WE KEEP GOING IN THE SAME DIRECTION OR NOT?
                dir = Utils.Roll(sameDirectionOdds) ? primaryDirection : GetRandomDir();
                if (tileCounter >= tilesForRoomChance)
                {
                    tileCounter = 0;
                    if (SpawnRoom(tiles, pos, center))
                        return;
                }

                // KEEP GOING UNTIL YOU FIND A VALID TILE.
                while (Utils.Has(pos.x, pos.y, tiles, width, height))
                    pos += dir;

                if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
                    return;

                if (Vector2Int.Distance(pos, Vector2Int.zero) > maxDistance)
                    return;

                tiles[pos.x, pos.y] = floor;
                tileCounter++;
            }
        }

        /// <summary>
        /// Use texture to spawn TileType room layout with enemies.
        /// </summary>
        /// <returns>Terminate walker.</returns>
        private bool SpawnRoom(TileType[,] tiles, Vector2Int pos, Vector2Int center)
        {
            if (!Utils.Roll(roomOdds))
                return false;

            Room room = rooms[Random.Range(0, rooms.Length)];
            room.Apply(tiles, pos.x, pos.y, center, colorToType);
            return room.terminateWalkAfter;
        }
    }
}