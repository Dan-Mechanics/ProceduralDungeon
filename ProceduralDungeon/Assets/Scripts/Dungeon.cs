using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    /// <summary>
    /// Master script.
    /// </summary>
    public class Dungeon : MonoBehaviour
    {
        private Blackboard blackboard;
        private IDungeonGenerator generator;
        private List<IDungeonDecorator> decorators;
        private IDungeonVisualizer visualizer; 

        public void Setup(Blackboard blackboard)
        {
            this.blackboard = blackboard;
            generator = GetComponent<IDungeonGenerator>();
            visualizer = GetComponent<IDungeonVisualizer>();
            decorators = GetComponents<IDungeonDecorator>().ToList();
        }

        public void Refresh()
        {
            // REMOVE OLD STUFF.
            Hide();

            TileType[,] tiles = generator.Generate(blackboard);
            decorators.ForEach(x => x.Decorate(tiles, blackboard));
            visualizer.Visualize(tiles);
        }

        public void Hide() 
        {
            List<GameObject> tiles = GameObject.FindGameObjectsWithTag("Tile").ToList();
            tiles.ForEach(x => DestroyImmediate(x));
        }
    }
}
