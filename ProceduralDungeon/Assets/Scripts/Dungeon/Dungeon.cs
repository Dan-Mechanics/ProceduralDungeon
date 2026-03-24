using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private readonly List<IContentPlacer> contentPlacers = new List<IContentPlacer>();
        private IDungeonVisualizer visualizer;

        public void Setup()
        {
            generator = FindAnyObjectByType<RandomWalk>();
            analyzer = FindAnyObjectByType<FloodFill>();
            contentPlacers.Add(FindAnyObjectByType<ContentPlacer>());

            visualizer = FindAnyObjectByType<SpriteRendererDungeon>();  
        }

        public void Generate(Blackboard blackboard)
        {
            TileType[,] tiles = generator.Generate(blackboard);
            TileMetadata[,] metadata = analyzer.Analyze(tiles);
            contentPlacers.ForEach(x => x.PlaceContent(tiles, metadata, blackboard));
            visualizer.Visualize(tiles);
        }
    }
}
