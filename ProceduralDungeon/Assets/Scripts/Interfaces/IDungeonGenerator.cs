using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public interface IDungeonGenerator
    {
        public Dictionary<Vector2Int, TileType> Generate(Blackboard blackboard); 
    }
}