namespace ProceduralDungeon
{
    public interface IContentPlacer
    {
        public void PlaceContent(TileType[,] tiles, TileMetadata[,] metadata, Blackboard blackboard); 
    }
}