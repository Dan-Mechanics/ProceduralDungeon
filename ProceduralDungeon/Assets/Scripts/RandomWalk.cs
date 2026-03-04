using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    [System.Serializable]
    public class RandomWalk : ILayoutGenerator 
    {
        public int seed;
        public int noiseLevel;

        public Tile[,] Generate()
        {
            throw new System.NotImplementedException();
        }
    }
}