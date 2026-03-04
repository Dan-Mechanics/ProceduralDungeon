using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public interface ILayoutDecorator
    {
        public void Decorate(Tile[,] tiles); 
    }
}