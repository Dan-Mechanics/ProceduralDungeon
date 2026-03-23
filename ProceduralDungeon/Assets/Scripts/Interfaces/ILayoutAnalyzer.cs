namespace ProceduralDungeon
{
    public interface ILayoutAnalyzer
    {
        public TileMetadata[,] Analyze(TileType[,] tiles);
    }
}