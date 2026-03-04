using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public struct Tile 
    {
        /// <summary>
        /// Or you could make this a sprite.
        /// </summary>
        public TileType type;

        public float width;
        public float height;
        public int x;
        public int y;
    }
}