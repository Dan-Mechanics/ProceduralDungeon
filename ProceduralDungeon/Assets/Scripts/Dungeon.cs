using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        [SerializeField] private RandomWalk randomWalk = default;
        [SerializeField] private Analyzer analyzer = default;
        [SerializeField] private ContentPlacer contentPlacer = default;

        public void ShowPreview()
        {
            HidePreview();
            Tile[,] tiles = GetTiles(randomWalk, new List<ILayoutDecorator>() { analyzer, contentPlacer });
            // visualize tiles.
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                // etc
            }
        }

        public void HidePreview() 
        {
            // destroy all tiles.
        }

        private Tile[,] GetTiles(ILayoutGenerator generator, List<ILayoutDecorator> decorators)
        {
            Tile[,] tiles = generator.Generate();
            decorators.ForEach(x => x.Decorate(tiles));
            return tiles;
        }
    }
}
