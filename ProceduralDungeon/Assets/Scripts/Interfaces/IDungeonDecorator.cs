using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public interface IDungeonDecorator
    {
        public void Decorate(Dictionary<Vector2Int, TileType> tiles, Blackboard blackboard); 
    }
}