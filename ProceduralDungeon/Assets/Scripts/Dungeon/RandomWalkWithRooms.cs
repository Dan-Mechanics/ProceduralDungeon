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
        [SerializeField] private RoomType safeRooms = default;
        [SerializeField] private RoomType enemyRooms = default;
        private Dictionary<Color, TileType> colorToType;

        private float sameDirectionOdds;
        private float maxDistance;
        private int iterations;
        private float safeRoomPercentage;
        private float roomChance;
        private int iterationsForRoom;
        private readonly Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
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
            ConfigureParameters(blackboard);

            TileType[,] tiles = new TileType[width, height];
            Vector2Int center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));
            tiles[center.x, center.y] = floor;

            SendWalker(tiles, center, Vector2Int.right);
            SendWalker(tiles, center, Vector2Int.up);
            SendWalker(tiles, center, Vector2Int.down);
            SendWalker(tiles, center, Vector2Int.left);

            List<Vector2Int> activeTiles = new List<Vector2Int>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tiles[x, y] != null)
                        activeTiles.Add(new Vector2Int(x, y));
                }
            }

            EnforceMinRoomConstraint(enemyRooms, activeTiles, tiles, center);
            EnforceMinRoomConstraint(safeRooms, activeTiles, tiles, center);
            return tiles;
        }

        private void EnforceMinRoomConstraint(RoomType roomType, List<Vector2Int> activeTiles, TileType[,] tiles, Vector2Int center)
        {
            while (roomType.count < roomType.minRequired)
            {
                int index = Random.Range(0, activeTiles.Count);
                SpawnRoom(tiles, activeTiles[index], center, roomType);
                activeTiles.RemoveAt(index);
            }
        }

        private void ConfigureParameters(Blackboard blackboard)
        {
            sameDirectionOdds = blackboard.GetValue<float>(nameof(sameDirectionOdds));
            iterations = blackboard.GetValue<int>(nameof(iterations));
            maxDistance = blackboard.GetValue<float>(nameof(maxDistance));
            iterationsForRoom = blackboard.GetValue<int>(nameof(iterationsForRoom));
            roomChance = blackboard.GetValue<float>(nameof(roomChance));
            safeRoomPercentage = blackboard.GetValue<float>(nameof(safeRoomPercentage));

            safeRooms.minRequired = blackboard.GetValue<int>($"{nameof(safeRooms)}Required");
            enemyRooms.minRequired = blackboard.GetValue<int>($"{nameof(enemyRooms)}Required");

            string seed = blackboard.GetValue<string>(nameof(seed));
            if (!Utils.IsStringValid(seed))
                seed = "default";

            Random.InitState(seed.GetHashCode());

            if (colorToType == null)
                colorToType = GetColorToType();

            safeRooms.Clear();
            enemyRooms.Clear();
        }

        private void SendWalker(TileType[,] tiles, Vector2Int center, Vector2Int primaryDirection) 
        {
            Vector2Int pos = center;
            int stepsTaken = 0;
            for (int i = 0; i < iterations; i++)
            {
                // DO WE KEEP GOING IN THE SAME DIRECTION OR NOT?
                Vector2Int dir = Utils.Roll(sameDirectionOdds) ? primaryDirection : GetRandomDir();
                if (stepsTaken >= iterationsForRoom)
                {
                    stepsTaken = 0;
                    if (Utils.Roll(roomChance))
                    {
                        RoomType roomType = Utils.Roll(safeRoomPercentage) ? safeRooms : enemyRooms;
                        SpawnRoom(tiles, pos, center, roomType);
                    }
                }

                // KEEP GOING UNTIL YOU FIND A VALID TILE.
                while (Utils.Has(pos.x, pos.y, tiles, width, height))
                {
                    pos += dir;
                }

                if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
                    return;

                if (Vector2Int.Distance(pos, Vector2Int.zero) > maxDistance)
                    return;

                tiles[pos.x, pos.y] = floor;
                stepsTaken++;
            }
        }

        private void SpawnRoom(TileType[,] tiles, Vector2Int pos, Vector2Int center, RoomType roomType)
        {
            roomType.count++;
            Room room = roomType.GetRandomRoom();
            room.Apply(tiles, pos.x, pos.y, center, colorToType);
        }

        [System.Serializable]
        public class RoomType
        {
            public Room[] rooms;
            [HideInInspector] public int minRequired;
            [HideInInspector] public int count;

            public void Clear() => count = 0;
            public Room GetRandomRoom() => rooms[Random.Range(0, rooms.Length)];
        }
    }
}