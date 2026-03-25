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

        private Vector2Int center;
        private float sameDirectionOdds;
        private float maxDistance;
        private int iterations;
        private float roomChance;
        private float stepsForRoom;

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

            center = new Vector2Int(Mathf.RoundToInt(width / 2f), Mathf.RoundToInt(height / 2f));

            sameDirectionOdds = 0.3f;
            iterations = 80;
            maxDistance = 112f;
            stepsForRoom = 10;
            roomChance = 0.5f;

            // TODO: MAKE THESE ALL BLACKBOARD VALUES !!
            TileType[,] tiles = new TileType[width, height];
            tiles[center.x, center.y] = floor;

            SendWalker(tiles, center, Vector2Int.right);
            SendWalker(tiles, center, Vector2Int.up);
            SendWalker(tiles, center, Vector2Int.down);
            SendWalker(tiles, center, Vector2Int.left);
            return tiles;
        }

        private void SendWalker(TileType[,] tiles, Vector2Int startingPosition,  Vector2Int startingDirection) 
        {
            Vector2Int pos = startingPosition;
            Vector2Int dir;
            int steps = 0;
            for (int i = 0; i < iterations; i++)
            {
                if(Random.value < sameDirectionOdds)
                {
                    dir = startingDirection;
                }
                else
                {
                    dir = GetRandomDir();
                }

                steps++;

                if (steps > stepsForRoom)
                {
                    steps = 0;
                    if (Random.value > roomChance)
                        continue;

                    Room room = rooms[Random.Range(0, rooms.Length)];
                    room.Apply(tiles, pos.x, pos.y, center, colorToType);
                    if (room.terminateAfterRoom)
                        return;
                }

                while (Utils.Has(pos.x, pos.y, tiles, width, height))
                {
                    pos += dir;
                }

                if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
                    return;

                if (Vector2Int.Distance(pos, Vector2Int.zero) > maxDistance)
                    return;

                tiles[pos.x, pos.y] = floor;
            }
        }
    }
}