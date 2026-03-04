using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class ContentPlacer : ILayoutDecorator 
    {
        public int seed;
        public int noiseLevel;

        public void Decorate(Tile[,] tiles)
        {
            throw new System.NotImplementedException();
        }
    }
}