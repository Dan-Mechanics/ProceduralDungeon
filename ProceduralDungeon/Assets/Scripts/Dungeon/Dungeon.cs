using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private readonly List<IFinalizer> finalizers = new List<IFinalizer>();
        private IDungeonVisualizer visualizer;

        public void Setup()
        {
            generator = FindAnyObjectByType<RandomWalk>();
            analyzer = FindAnyObjectByType<FloodFill>();
            finalizers.Add(FindAnyObjectByType<IslandRemover>());
           // finalizers.Add(FindAnyObjectByType<NeighboursDebug>());
            finalizers.Add(FindAnyObjectByType<ContentPlacer>());
            // MANY MORE THINGS CAN GO HERE ...

            visualizer = FindAnyObjectByType<SpriteRendererDungeon>();  
        }

        public void Generate(Blackboard blackboard)
        {
            TileType[,] tiles = generator.Generate(blackboard);
            TileMetadata[,] metadata = analyzer.Analyze(tiles);
            finalizers.ForEach(x => x.Finalize(tiles, metadata, blackboard));
            visualizer.Visualize(tiles);
        }
    }
}
