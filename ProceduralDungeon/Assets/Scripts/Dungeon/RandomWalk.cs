using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class RandomWalk : MonoBehaviour, IDungeonGenerator 
    {
        [SerializeField] private TileType stone = default;
    //    [SerializeField] private int width = default;
    //    [SerializeField] private int height = default;
    //    private bool hasPortalRoom;
    //    private int libraryCount;

        private Vector2Int GetRandomDir()
        {
            int dir = Random.Range(0, 4);
            Vector2Int walkerPos = dir switch
            {
                0 => Vector2Int.up,
                1 => Vector2Int.right,
                2 => Vector2Int.left,
                3 => Vector2Int.down,
                _ => throw new System.Exception(),
            };

            return walkerPos;
        }

        public Dictionary<Vector2Int, TileType> Generate(Blackboard blackboard)
        {
            string seed = blackboard.GetValue<string>(nameof(seed));
            if (!Utils.IsStringValid(seed))
                seed = "default";

            Random.InitState(seed.GetHashCode());

            // we're gonna use temp hard code until it works then implement blackbaord.

            Dictionary<Vector2Int, TileType> tiles = new Dictionary<Vector2Int, TileType>();
            SendWalker(tiles, Vector2Int.right, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.up, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.down, 0.5f, 80, 112f);
            SendWalker(tiles, Vector2Int.left, 0.5f, 80, 112f);
            return tiles;
        }

        private void SendWalker(Dictionary<Vector2Int, TileType> tiles, Vector2Int direction, float sameDirectionOdds, int doneRooms, float doneDistance)
        {
            Vector2Int walkerPos = Vector2Int.zero;
            Vector2Int currentDirection = direction;
            for (int i = 0; i < doneRooms; i++)
            {
                if (Random.value < sameDirectionOdds)
                    currentDirection = GetRandomDir();

                walkerPos += currentDirection;
                while (tiles.ContainsKey(walkerPos))
                {
                    currentDirection = GetRandomDir();
                    walkerPos += currentDirection;
                }

                tiles.Add(walkerPos, stone);
                if (Vector2Int.Distance(walkerPos, Vector2Int.zero) > doneDistance)
                    return;
            }
        }
    }
}