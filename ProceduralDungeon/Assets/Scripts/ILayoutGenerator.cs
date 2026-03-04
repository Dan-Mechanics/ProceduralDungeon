using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public interface ILayoutGenerator
    {
        public Tile[,] Generate(); 
    }
}