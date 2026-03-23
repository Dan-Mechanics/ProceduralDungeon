using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private IFinalizer contentPlacer;
        private IDungeonVisualizer visualizer;

        /// <summary>
        /// TODO MAKE FINALIZER LIST TYPE BEAT
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="analyzer"></param>
        /// <param name="contentPlacer"></param>
        /// <param name="visualizer"></param>
        public void Setup(ILayoutGenerator generator, ILayoutAnalyzer analyzer, IFinalizer contentPlacer, IDungeonVisualizer visualizer)
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
            contentPlacer.Finalize(tiles, metadata, blackboard);
            visualizer.Visualize(tiles);
        }
    }
}
