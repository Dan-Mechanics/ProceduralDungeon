using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        [SerializeField] private string tileTag = default;
        [SerializeField] private RandomWalk randomWalk = default;
        [SerializeField] private LayoutAnalyzer layoutAnalyzer = default;
        [SerializeField] private ContentPlacer contentPlacer = default;

        public void Show()
        {
            if (!Hide())
                return;

            Tile[,] tiles = GetTiles(randomWalk, new List<ILayoutDecorator>() { layoutAnalyzer, contentPlacer });
            GetComponent<IDungeonVisualizer>().Visualize(tiles);
        }

        public bool Hide() 
        {
            if (string.IsNullOrEmpty(tileTag) || string.IsNullOrWhiteSpace(tileTag) || tileTag == "Untagged") 
            {
                Debug.LogError($"Please correctly assign {nameof(tileTag)}. It is currently set to '{tileTag}'.");
                return false;
            }
            
            List<GameObject> tiles = GameObject.FindGameObjectsWithTag(tileTag).ToList();
            tiles.ForEach(x => DestroyImmediate(x));
            return true;
        }

        private Tile[,] GetTiles(ILayoutGenerator generator, List<ILayoutDecorator> decorators)
        {
            Tile[,] tiles = generator.Generate();
            decorators.ForEach(x => x.Decorate(tiles));
            return tiles;
        }
    }
}
