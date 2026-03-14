using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private Blackboard blackboard;
        private IDungeonGenerator generator;
    //    private List<IDungeonDecorator> decorators;
        private IDungeonVisualizer visualizer; 

        public void Setup(Blackboard blackboard)
        {
            this.blackboard = blackboard;
            generator = GetComponent<IDungeonGenerator>();
           // decorators = GetComponents<IDungeonDecorator>().ToList();
            visualizer = GetComponent<IDungeonVisualizer>();
        }

        public void Refresh()
        {
            Dictionary<Vector2Int, TileType> tiles = generator.Generate(blackboard);
          //  decorators.ForEach(x => x.Decorate(tiles, blackboard));
            visualizer.Refresh(tiles);
        }
    }
}
