using UnityEngine;

namespace ProceduralDungeon
{
    public class Dungeon : MonoBehaviour
    {
        private ILayoutGenerator generator;
        private ILayoutAnalyzer analyzer;
        private IContentPlacer contentPlacer;
        private IDungeonVisualizer visualizer;

        public void Setup()
        {
            generator = FindAnyObjectByType<RandomWalkWithRooms>();
            analyzer = FindAnyObjectByType<FloodFill>();
            contentPlacer = FindAnyObjectByType<ContentPlacer>();
            visualizer = FindAnyObjectByType<SpriteRendererDungeon>();  
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
