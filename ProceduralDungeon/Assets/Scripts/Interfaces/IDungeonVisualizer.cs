using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public interface IDungeonVisualizer
    {
        void Refresh(Dictionary<Vector2Int, TileType> tiles);
    }
}