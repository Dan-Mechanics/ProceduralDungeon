using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private IContentPlacer contentPlacer;
        private IDungeonVisualizer visualizer;

        public void Setup(ILayoutGenerator generator, ILayoutAnalyzer analyzer, IContentPlacer contentPlacer, IDungeonVisualizer visualizer)
        {
            this.generator = generator;
            this.analyzer = analyzer;
            this.contentPlacer = contentPlacer;
            this.visualizer = visualizer;
        }

        public void Generate(Blackboard blackboard)
        {
            TileType[,] tiles = generator.Generate(blackboard);
            TileMetadata[,] metadata = analyzer.Analyze(tiles);
            contentPlacer.PlaceContent(tiles, metadata, blackboard);
            visualizer.Visualize(tiles);
        }
    }
}
